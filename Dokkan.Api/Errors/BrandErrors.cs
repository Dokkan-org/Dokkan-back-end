using Dokkan.Api.Abstractions;

namespace Dokkan.Api.Errors;

public record BrandErrors
{
    public static readonly Error NotFound 
        = new ("Brand.NotFound", "Brand not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedBrand =
        new("Brand.DuplicatedBrand", "Another brand with the same name already exists", StatusCodes.Status400BadRequest);
}
