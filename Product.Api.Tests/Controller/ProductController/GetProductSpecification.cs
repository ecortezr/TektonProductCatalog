using System.Net;
using System.Net.Http.Json;

namespace Product.Api.Tests.Controller.ProductController
{
    public class GetProductSpecification : BaseSpecification
    {
        [Fact]
        public async Task Should_Return_404_When_Product_Not_Found()
        {
            var response = await _httpClient.GetAsync("/Product/GetById/1");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_Return_200_When_A_Product_Is_Found()
        {
            var addResponse = await _httpClient.PostAsJsonAsync("/Product/Insert", new
            {
                Name = "Test Name",
                Description = "Test Description",
                Status = 1,
                Stock = 0,
                Price = 10
            });

            Assert.Equal(HttpStatusCode.Created, addResponse.StatusCode);

            var response = await _httpClient.GetAsync("/Product/GetById/1");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}