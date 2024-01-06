using FluentValidation;

namespace Product.Api.Validators
{
    public class ProductValidator : AbstractValidator<Domain.Product>
    {
        public ProductValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty()
                .WithMessage("Invalid Name");
        }
    }
}
