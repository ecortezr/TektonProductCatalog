using FluentValidation.TestHelper;
using Product.Api.Validators;

namespace Product.Api.Tests.Validators
{
    public class ProductValidatorTests
    {
        private readonly ProductValidator _validator = new();

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GivenAnInvalidNameValue_ShouldHaveValidationError(string name)
        {
            var product = new Domain.Product { Name = name };
            var result = _validator.TestValidate(product);

            result.ShouldHaveValidationErrorFor(model => model.Name);
        }
    }
}
