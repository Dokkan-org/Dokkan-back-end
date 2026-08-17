using Dokkan.Api.Abstractions;
using Dokkan.Api.Contracts.Category;
using Dokkan.Api.Contracts.Common;

namespace Dokkan.Api.Services;

public interface ICategoryService
{
    Task<Result<PaginatedList<CategoryResponse>>> GetAllAsync(RequestFilters filters, CancellationToken cancellationToken = default);
    Task<Result<PaginatedList<CategoryResponse>>> GetAvailableAsync(RequestFilters filters, CancellationToken cancellationToken = default);
    Task<Result<CategoryResponse>> GetAsync(int id, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(int id, CategoryRequest request, CancellationToken cancellationToken = default);
    Task<Result<CategoryResponse>> CreateAsync(CategoryRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleActiveStatus(int id, CancellationToken cancellationToken = default);
}
