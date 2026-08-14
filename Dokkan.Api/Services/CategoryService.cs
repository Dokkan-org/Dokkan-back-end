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

public class CategoryService(ApplicationDbContext context) : ICategoryService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<PaginatedList<CategoryResponse>>> GetAllAsync(RequestFilters filters,bool? isActive=null, CancellationToken cancellationToken = default)
    {
        IQueryable<Category> query = _context.Categories
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
            .ProjectToType<CategoryResponse>();

        var categories = await PaginatedList<CategoryResponse>.CreateAsync(source, filters.PageSize, filters.PageNumber, cancellationToken);

        return Result.Success(categories);
    }


    public async Task<Result<CategoryResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var category= await _context.Categories
            .Where(x=>x.Id==id)
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null)
            return Result.Failure<CategoryResponse>(CategoryErrors.NotFound);

        var response = category.Adapt<CategoryResponse>();

        return Result.Success(response);
    }

    public async Task<Result<CategoryResponse>> CreateAsync(CategoryRequest request, CancellationToken cancellationToken = default)
    {
        var categoryIsExisted= await _context.Categories.AnyAsync(x=>x.Name==request.Name,cancellationToken);

        if (categoryIsExisted)
            return Result.Failure<CategoryResponse>(CategoryErrors.DuplicatedCategory);

        var category = request.Adapt<Category>();

        await _context.AddAsync(category,cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(category.Adapt<CategoryResponse>());
    }

    public async Task<Result> UpdateAsync(int id,CategoryRequest request, CancellationToken cancellationToken = default)
    {
        var isCategoryExisted = await _context.Categories
            .AnyAsync(x => x.Id != id && x.Name == request.Name,cancellationToken);

        if (isCategoryExisted)
            return Result.Failure(CategoryErrors.DuplicatedCategory);

        var category = await _context.Categories
            .Where(x => x.Id == id)
            .SingleOrDefaultAsync(cancellationToken);

        if (category is null)
            return Result.Failure(CategoryErrors.NotFound);

        request.Adapt(category); 

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result> ToggleActiveStatus(int id,CancellationToken cancellationToken=default)
    {
        var category= await _context.Categories
            .Where(x=>x.Id == id)
            .SingleOrDefaultAsync(cancellationToken);

        if (category is null)
            return Result.Failure(CategoryErrors.NotFound);

        category.IsActive = !category.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
