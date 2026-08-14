namespace Dokkan.Api.Contracts.Category;

public sealed record CategoryResponse
    (int Id,
    string Name,
    string Description,
    bool IsActive);
