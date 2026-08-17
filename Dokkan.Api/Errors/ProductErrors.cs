using Dokkan.Api.Abstractions;

namespace Dokkan.Api.Errors;

public record ProductErrors
{
    public static readonly Error NotFound 
        = new ("Product.NotFound", "Product not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedProduct =
       new("Product.DuplicatedProduct", "Another Product with the same name in same brand already exists", StatusCodes.Status400BadRequest);
}
