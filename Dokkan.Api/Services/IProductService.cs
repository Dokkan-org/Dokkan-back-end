using Dokkan.Api.Abstractions;
using Dokkan.Api.Contracts.Common;
using Dokkan.Api.Contracts.Products;

namespace Dokkan.Api.Services;

public interface IProductService
{
    Task<Result<PaginatedList<ProductResponse>>> GetAllAsync(ProductFilters productFilters, CancellationToken cancellationToken = default);
    Task<Result<PaginatedList<ProductResponse>>> GetAvailableAsync(ProductFilters productFilters, CancellationToken cancellationToken = default);
    Task<Result<ProductResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<ProductResponse>> AddAsync(ProductRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id,ProductRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleActiveAsync(int id, CancellationToken cancellationToken = default);
    //( toggle) product 
}
