using FluentValidation;

namespace Dokkan.Api.Contracts.Products;

public class ProductRequestValidator:AbstractValidator<ProductRequest>
{
    public ProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .Length(3, 250)
            .WithMessage("Name must be between 3 and 250 characters");

        RuleFor(x => x.Description)
            .NotNull()
            .NotEmpty()
            .Length(3, 1500)
            .WithMessage("Description must be between 5 and 1500 characters");

        RuleFor(x => x.BasePrice)
            .NotNull()
            .NotEmpty()
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be greater than or equal zero");

        RuleFor(x => x.BrandId)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("BrandId must be greater than zero");

        RuleFor(x => x.CategoryId)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("CategoryId must be greater than zero");

    }
}
