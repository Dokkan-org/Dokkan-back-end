namespace Dokkan.Api.Contracts.Category;

public sealed record BrandResponse
    (int Id,
    string Name,
    string Description,
    bool IsActive);
