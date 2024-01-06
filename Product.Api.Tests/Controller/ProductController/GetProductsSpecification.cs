using System.Net;

namespace Product.Api.Tests.Controller.ProductController
{
    public class GetProductsSpecification
    {
        private readonly ProductApiFactory _api;
        private readonly HttpClient _httpClient;

        public GetProductsSpecification()
        {
            _api = new ProductApiFactory();
            _httpClient = _api.CreateClient();
        }

        [Fact]
        public async Task Should_Return_200_When_Is_Hitted()
        {
            var response = await _httpClient.GetAsync("/Product");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}