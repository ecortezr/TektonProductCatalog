using FluentValidation.TestHelper;
using Product.Api.Domain.Features.Products.Commands;
using Product.Api.Domain.Validators;

namespace Product.Api.Tests.Validators
{
    public class UpdateBodyProductCommandValidatorTests
    {
        private readonly UpdateBodyProductCommandValidator _validator = new();

        [Fact]
        public void GivenAnInvalidNameValue_ShouldHaveValidationError()
        {
            var product = new UpdateBodyProductCommand { Name = "" };
            var result = _validator.TestValidate(product);

            result.ShouldHaveValidationErrorFor(model => model.Name);
        }
        
        [Fact]
        public void GivenAnInvalidDescriptionValue_ShouldHaveValidationError()
        {
            var product = new UpdateBodyProductCommand { Description = "" };
            var result = _validator.TestValidate(product);

            result.ShouldHaveValidationErrorFor(model => model.Description);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(5)]
        public void GivenInvalidStatusValue_ShouldHaveValidationError(int status)
        {
            var product = new UpdateBodyProductCommand { Status = status };
            var result = _validator.TestValidate(product);

            result.ShouldHaveValidationErrorFor(model => model.Status);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void GivenInvalidPriceValue_ShouldHaveValidationError(decimal price)
        {
            var product = new UpdateBodyProductCommand { Price = price };
            var result = _validator.TestValidate(product);

            result.ShouldHaveValidationErrorFor(model => model.Price);
        }
    }
}
