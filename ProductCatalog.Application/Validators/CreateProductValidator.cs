using FluentValidation;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Application.Validators;

public class CreateProductValidator : AbstractValidator<ProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .Matches(@"^\d{4}-\d{4}$")
            .WithMessage("Product code must follow the format 'XXXX-XXXX', where X is a digit from 0 to 9");

        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 100)
            .WithMessage("Product name must be between 3 and 100 characters long");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Product description must not exceed 500 characters");

        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThan(0)
            .ScalePrecision(2, 18)
            .WithMessage("Product price must be greater than 0 and can have up to 2 decimal places");
    }
}