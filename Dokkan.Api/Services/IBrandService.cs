using Dokkan.Api.Abstractions;
using Dokkan.Api.Contracts.Category;
using Dokkan.Api.Contracts.Common;

namespace Dokkan.Api.Services;

public interface IBrandService
{
    Task<Result<PaginatedList<BrandResponse>>> GetAllAsync(RequestFilters filters, bool? isActive, CancellationToken cancellationToken = default);
    Task<Result<BrandResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, BrandRequest request, CancellationToken cancellationToken = default);
    Task<Result<BrandResponse>> CreateAsync(BrandRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleActiveStatus(int id, CancellationToken cancellationToken = default);
}
