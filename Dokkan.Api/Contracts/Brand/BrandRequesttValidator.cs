using FluentValidation;

namespace Dokkan.Api.Contracts.Brand;

public class BrandRequesttValidator : AbstractValidator<BrandRequest>
{
    public BrandRequesttValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1, 50)
            .WithMessage("Name must be between 1 and 50 characters");

        RuleFor(x => x.Description)
           .NotEmpty()
           .Length(5, 1500)
           .WithMessage("Description must be between 5 and 1500 characters");

    }
}
