﻿using FluentValidation;
using Product.Api.Domain.Features.Products.Commands;

namespace Product.Api.Domain.Validators
{
    public class UpdateBodyProductCommandValidator : AbstractValidator<UpdateBodyProductCommand>
    {
        public UpdateBodyProductCommandValidator()
        {
            When(product => product.Name != null, () => {
                RuleFor(product => product.Name).NotEmpty();
            });
            When(product => product.Description != null, () => {
                RuleFor(product => product.Description).NotEmpty();
            });
            RuleFor(product => product.Status)
                .InclusiveBetween(0, 1)
                .When(product => product.Status != null)
                .WithMessage("Invalid Status (must be 0 or 1)");
            RuleFor(product => product.Price)
                .GreaterThan(0)
                .When(product => product.Price != null)
                .WithMessage("Invalid Price (must be greather than 0)");
        }
    }
}
