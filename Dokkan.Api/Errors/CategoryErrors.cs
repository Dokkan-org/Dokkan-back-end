using Dokkan.Api.Abstractions;

namespace Dokkan.Api.Errors;

public record CategoryErrors
{
    public static readonly Error NotFound 
        = new ("Category.NotFound", "Category not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedCategory =
        new("Category.DuplicatedCategory", "Another category with the same name already exists", StatusCodes.Status400BadRequest);
}
