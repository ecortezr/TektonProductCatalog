using FluentValidation.TestHelper;
using Product.Api.Features.Products.Commands;
using Product.Api.Validators;

namespace Product.Api.Tests.Validators
{
    public class UpdateProductCommandValidatorTests
    {
        private readonly UpdateProductCommandValidator _validator = new();

        [Fact]
        public void GivenAnInvalidNameValue_ShouldHaveValidationError()
        {
            var product = new UpdateProductCommand { Name = "" };
            var result = _validator.TestValidate(product);

            result.ShouldHaveValidationErrorFor(model => model.Name);
        }
        
        [Fact]
        public void GivenAnInvalidDescriptionValue_ShouldHaveValidationError()
        {
            var product = new UpdateProductCommand { Description = "" };
            var result = _validator.TestValidate(product);

            result.ShouldHaveValidationErrorFor(model => model.Description);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(5)]
        public void GivenInvalidStatusValue_ShouldHaveValidationError(int status)
        {
            var product = new UpdateProductCommand { Status = status };
            var result = _validator.TestValidate(product);

            result.ShouldHaveValidationErrorFor(model => model.Status);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void GivenInvalidPriceValue_ShouldHaveValidationError(decimal price)
        {
            var product = new UpdateProductCommand { Price = price };
            var result = _validator.TestValidate(product);

            result.ShouldHaveValidationErrorFor(model => model.Price);
        }
    }
}
