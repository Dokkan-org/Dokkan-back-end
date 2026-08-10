using Dokkan.Api.Abstractions.Consts;
using FluentValidation;

namespace Dokkan.Api.Contracts.Authentication;


public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3, 50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3, 50);

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password Should be at least 8 digits, and contain Lowercase, Nonalphanumeric and Uppercase");
    }
}