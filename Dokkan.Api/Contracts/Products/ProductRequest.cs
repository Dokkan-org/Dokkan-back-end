using Dokkan.Api.Entities;

namespace Dokkan.Api.Contracts.Products;

public record ProductRequest
(
    string Name,
    string Description,
    int BrandId,
    int CategoryId,
    decimal BasePrice);
