using System.Net;

namespace Product.Api.Tests.Controller.ProductController
{
    public class GetProductsSpecification : BaseSpecification
    {
        [Fact]
        public async Task Should_Return_200_When_Is_Hitted()
        {
            var response = await _httpClient.GetAsync("/Product");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}