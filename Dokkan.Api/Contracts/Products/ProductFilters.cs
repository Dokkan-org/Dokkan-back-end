using Dokkan.Api.Contracts.Common;

namespace Dokkan.Api.Contracts.Products;

public record ProductFilters : RequestFilters
{
    public int? BrandId { get; init; }
    public int? CategoryId { get; init; }
}
