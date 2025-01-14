using FluentValidation;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Validators;

public class ProductCodeValidator : AbstractValidator<string>
{
    public ProductCodeValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .Matches(@"^\d{4}-\d{4}$")
            .WithMessage("Product code must follow the format 'XXXX-XXXX', where X is a digit from 0 to 9");
    }
}