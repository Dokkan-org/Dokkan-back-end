namespace Dokkan.Api.Contracts.Brand;

public sealed record BrandResponse
    (int Id,
    string Name,
    string Description,
    bool IsActive);
