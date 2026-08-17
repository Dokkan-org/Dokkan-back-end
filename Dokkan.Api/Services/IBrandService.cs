using Dokkan.Api.Abstractions;
using Dokkan.Api.Contracts.Brand;
using Dokkan.Api.Contracts.Common;

namespace Dokkan.Api.Services;

public interface IBrandService
{
    Task<Result<PaginatedList<BrandResponse>>> GetAllAsync(RequestFilters filters, CancellationToken cancellationToken = default);
    Task<Result<PaginatedList<BrandResponse>>> GetAvailableAsync(RequestFilters filters, CancellationToken cancellationToken = default);
    Task<Result<BrandResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, BrandRequest request, CancellationToken cancellationToken = default);
    Task<Result<BrandResponse>> CreateAsync(BrandRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleActiveStatus(int id, CancellationToken cancellationToken = default);
}
