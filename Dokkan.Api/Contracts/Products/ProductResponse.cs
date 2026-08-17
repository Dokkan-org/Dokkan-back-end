namespace Dokkan.Api.Contracts.Products;

public record ProductResponse
(
    int Id,
    string Name,
    string Description,
    bool IsActive,
    string Brand,
    string Category,
    decimal BasePrice
);
