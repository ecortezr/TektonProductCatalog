using FluentValidation;
using Product.Api.Domain.Features.Products.Commands;

namespace Product.Api.Domain.Validators
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty()
                .WithMessage("Invalid Name");
            RuleFor(product => product.Description)
                .NotEmpty()
                .WithMessage("Invalid Description");
            RuleFor(product => product.Status)
                .InclusiveBetween(0, 1)
                .WithMessage("Invalid Status (must be 0 or 1)");
            RuleFor(product => product.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Invalid Stock (must be greather than or equal to 0)");
            RuleFor(product => product.Price)
                .GreaterThan(0)
                .WithMessage("Invalid Stock (must be greather than 0)");
        }
    }
}
