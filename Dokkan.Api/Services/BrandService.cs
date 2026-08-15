using Dokkan.Api.Abstractions;
using Dokkan.Api.Contracts.Category;
using Dokkan.Api.Contracts.Common;
using Dokkan.Api.Entities;
using Dokkan.Api.Errors;
using Dokkan.Api.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;


namespace Dokkan.Api.Services;

public class BrandService(ApplicationDbContext context) : IBrandService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<PaginatedList<BrandResponse>>> GetAllAsync(RequestFilters filters,bool? isActive=null, CancellationToken cancellationToken = default)
    {
        IQueryable<Brand> query = _context.Brands
            .AsNoTracking();
            
  
        if(isActive.HasValue)
            query=query.Where(x=>x.IsActive==isActive.Value);

        if (!string.IsNullOrEmpty(filters.SearchValue))
        {
            query = query.Where(x => x.Name.Contains(filters.SearchValue));
        }

        if (!string.IsNullOrEmpty(filters.SortColumn))
        {
            query = query.OrderBy($"{filters.SortColumn} {filters.SortDirection}");
        }

        var source = query
            .ProjectToType<BrandResponse>();

        var brands = await PaginatedList<BrandResponse>.CreateAsync(source, filters.PageSize, filters.PageNumber, cancellationToken);

        return Result.Success(brands);
    }


    public async Task<Result<BrandResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var category= await _context.Brands
            .Where(x=>x.Id==id)
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null)
            return Result.Failure<BrandResponse>(BrandErrors.NotFound);

        var response = category.Adapt<BrandResponse>();

        return Result.Success(response);
    }

    public async Task<Result<BrandResponse>> CreateAsync(BrandRequest request, CancellationToken cancellationToken = default)
    {
        var brandIsExisted= await _context.Brands.AnyAsync(x=>x.Name==request.Name,cancellationToken);

        if (brandIsExisted)
            return Result.Failure<BrandResponse>(BrandErrors.DuplicatedBrand);

        var brand = request.Adapt<Brand>();

        await _context.AddAsync(brand,cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(brand.Adapt<BrandResponse>());
    }

    public async Task<Result> UpdateAsync(int id,BrandRequest request, CancellationToken cancellationToken = default)
    {
        var isBrandExisted = await _context.Brands
            .AnyAsync(x => x.Id != id && x.Name == request.Name,cancellationToken);

        if (isBrandExisted)
            return Result.Failure(BrandErrors.DuplicatedBrand);

        var brand = await _context.Brands
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync(cancellationToken);

        if (brand is null)
            return Result.Failure(BrandErrors.NotFound);

        request.Adapt(brand); 

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> ToggleActiveStatus(int id,CancellationToken cancellationToken=default)
    {
        var brand= await _context.Brands
            .Where(x=>x.Id == id)
            .SingleOrDefaultAsync(cancellationToken);

        if (brand is null)
            return Result.Failure(BrandErrors.NotFound);

        brand.IsActive = !brand.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
