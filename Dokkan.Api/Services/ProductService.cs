using Dokkan.Api.Abstractions;
using Dokkan.Api.Contracts.Common;
using Dokkan.Api.Contracts.Products;
using Dokkan.Api.Entities;
using Dokkan.Api.Errors;
using Dokkan.Api.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;


namespace Dokkan.Api.Services;

public class ProductService(ApplicationDbContext applicationDbContext) : IProductService
{
    private readonly ApplicationDbContext _context = applicationDbContext;

    public async Task<Result<PaginatedList<ProductResponse>>> GetAllAsync(ProductFilters productFilters, CancellationToken cancellationToken = default)
    {
        IQueryable<Product> query = _context.Products
            .AsNoTracking();

        if(!string.IsNullOrEmpty(productFilters.SearchValue))
        {
            query=query.Where(x=>x.Name.Contains(productFilters.SearchValue));
        }

        if (!string.IsNullOrEmpty(productFilters.SortColumn))
        {
            query = query.OrderBy($"{productFilters.SortColumn} {productFilters.SortDirection}");
            
        }

        if (productFilters.BrandId.HasValue)
        {
            query = query.Where(x => x.BrandId == productFilters.BrandId.Value);
        }
        if (productFilters.CategoryId.HasValue)
        {
            query = query.Where(x => x.CategoryId == productFilters.CategoryId.Value);
        }

        var source = query
            .ProjectToType<ProductResponse>();
            

        var products = await PaginatedList<ProductResponse>.CreateAsync(source, productFilters.PageSize, productFilters.PageNumber, cancellationToken);

        return Result.Success(products);

    }

    public async Task<Result<PaginatedList<ProductResponse>>> GetAvailableAsync(ProductFilters productFilters, CancellationToken cancellationToken = default)
    {
        IQueryable<Product> query = _context.Products
            .AsNoTracking()
            .Where(x=>x.IsActive);

        if (!string.IsNullOrEmpty(productFilters.SearchValue))
        {
            query = query.Where(x => x.Name.Contains(productFilters.SearchValue));
        }

        if (!string.IsNullOrEmpty(productFilters.SortColumn))
        {
            query = query.OrderBy($"{productFilters.SortColumn} {productFilters.SortDirection}");

        }

        if (productFilters.BrandId.HasValue)
        {
            query = query.Where(x => x.BrandId == productFilters.BrandId.Value);
        }
        if (productFilters.CategoryId.HasValue)
        {
            query = query.Where(x => x.CategoryId == productFilters.CategoryId.Value);
        }

        var source = query
            .ProjectToType<ProductResponse>();

        var products = await PaginatedList<ProductResponse>.CreateAsync(source, productFilters.PageSize, productFilters.PageNumber, cancellationToken);

        return Result.Success(products);

    }

    public async Task<Result<ProductResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var product= await _context.Products
            .Where(x=>x.Id==id&&x.IsActive)
            .ProjectToType<ProductResponse>()
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
            return Result.Failure<ProductResponse>(ProductErrors.NotFound);

        return Result.Success(product);
    }

    public async Task<Result<ProductResponse>> AddAsync(ProductRequest request, CancellationToken cancellationToken = default)
    {
        var isCategoryExisted = await _context.Categories
            .AnyAsync(x => x.Id == request.CategoryId && x.IsActive,cancellationToken);

        if (!isCategoryExisted)
            return Result.Failure<ProductResponse>(CategoryErrors.NotFound);

        var isBrandExisted = await _context.Brands
            .AnyAsync(x => x.Id == request.BrandId && x.IsActive,cancellationToken);

        if (!isBrandExisted)
            return Result.Failure<ProductResponse>(BrandErrors.NotFound);

        var isProductExisted= await _context.Products
            .AnyAsync(x=>x.IsActive&&x.BrandId==request.BrandId&&x.Name==request.Name,cancellationToken);

        if (isProductExisted)
            return Result.Failure<ProductResponse>(ProductErrors.DuplicatedProduct);

        var product = request.Adapt<Product>();

        await _context.AddAsync(product,cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        var response= await _context.Products
            .Where(x => x.Id == product.Id)
            .ProjectToType<ProductResponse>()
            .SingleOrDefaultAsync(cancellationToken);

        if (response is null)
            return Result.Failure<ProductResponse>(ProductErrors.NotFound);

        return Result.Success(response);
    }

    public async Task<Result> UpdateAsync(int id,ProductRequest request, CancellationToken cancellationToken = default)
    {
        var isProductExisted = await _context.Products
            .AnyAsync(x => x.Id == id && x.IsActive,cancellationToken);

        if(!isProductExisted)
            return Result.Failure(ProductErrors.NotFound);

        var isCategoryExisted = await _context.Categories
            .AnyAsync(x => x.Id == request.CategoryId && x.IsActive,cancellationToken);

        if (!isCategoryExisted)
            return Result.Failure(CategoryErrors.NotFound);

        var isBrandExisted = await _context.Brands
            .AnyAsync(x => x.Id == request.BrandId && x.IsActive,cancellationToken);

        if (!isBrandExisted)
            return Result.Failure(BrandErrors.NotFound);

        var isProductExistedInBrand = await _context.Products
            .AnyAsync(x => x.IsActive && x.BrandId == request.BrandId && x.Name == request.Name, cancellationToken);

        if (isProductExistedInBrand)
            return Result.Failure(ProductErrors.DuplicatedProduct);

        var product= await _context.Products.FirstOrDefaultAsync(x => x.Id == id && x.IsActive,cancellationToken);

        request.Adapt(product);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> ToggleActiveAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await  _context.Products.FirstOrDefaultAsync(x => x.Id == id,cancellationToken);

        if (product is null)
            return Result.Failure(ProductErrors.NotFound);

        product.IsActive = !product.IsActive;
        
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}
