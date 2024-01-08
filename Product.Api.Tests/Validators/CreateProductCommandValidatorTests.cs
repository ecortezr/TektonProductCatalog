using FluentValidation.TestHelper;
using Product.Api.Features.Products.Commands;
using Product.Api.Validators;

namespace Product.Api.Tests.Validators
{
    public class CreateProductCommandValidatorTests
    {
        private readonly CreateProductCommandValidator _validator = new();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GivenAnInvalidNameValue_ShouldHaveValidationError(string name)
        {
            var product = new CreateProductCommand { Name = name };
            var result = _validator.TestValidate(product);

            result.ShouldHaveValidationErrorFor(model => model.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GivenAnInvalidDescriptionValue_ShouldHaveValidationError(string description)
        {
            var product = new CreateProductCommand { Description = description };
            var result = _validator.TestValidate(product);

            result.ShouldHaveValidationErrorFor(model => model.Description);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(5)]
        public void GivenInvalidStatusValue_ShouldHaveValidationError(int status)
        {
            var product = new CreateProductCommand { Status = status };
            var result = _validator.TestValidate(product);

            result.ShouldHaveValidationErrorFor(model => model.Status);
        }

        [Fact]
        public void GivenInvalidStockValue_ShouldHaveValidationError()
        {
            var product = new CreateProductCommand { Stock = -1 };
            var result = _validator.TestValidate(product);

            result.ShouldHaveValidationErrorFor(model => model.Stock);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void GivenInvalidPriceValue_ShouldHaveValidationError(decimal price)
        {
            var product = new CreateProductCommand { Price = price };
            var result = _validator.TestValidate(product);

            result.ShouldHaveValidationErrorFor(model => model.Price);
        }
    }
}
