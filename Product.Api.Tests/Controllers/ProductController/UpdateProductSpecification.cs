using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;

namespace Product.Api.Tests.Controllers.ProductController
{
    public class UpdateProductSpecification : BaseSpecification
    {
        const string PRODUCT_NAME = "Test 1";

        [Fact]
        public async Task Should_Return_400_When_Send_Invalid_Product_Data()
        {

            var response = await _httpClient.PutAsJsonAsync("/products/1", new
            {
                Name = ""
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Should_Return_404_When_Product_Not_Found()
        {
            var response = await _httpClient.PutAsJsonAsync("/products/1", new
            {
                Name = PRODUCT_NAME
            });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_Return_200_When_Update_Product()
        {
            var addResponse = await _httpClient.PostAsJsonAsync("/products", new
            {
                Name = PRODUCT_NAME,
                Description = "Test Description",
                Status = 1,
                Stock = 0,
                Price = 10
            });

            Assert.Equal(HttpStatusCode.Created, addResponse.StatusCode);

            var putResponse = await _httpClient.PutAsJsonAsync("/products/1", new
            {
                Name = $"{PRODUCT_NAME}-v2"
            });

            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
        }

        [Fact]
        public async Task Should_Update_Storage_When_Update_Product()
        {
            var addResponse = await _httpClient.PostAsJsonAsync("/products", new
            {
                Name = PRODUCT_NAME,
                Description = "Test Description",
                Status = 1,
                Stock = 0,
                Price = 10
            });

            Assert.Equal(HttpStatusCode.Created, addResponse.StatusCode);

            var newName = $"{PRODUCT_NAME}-v2";
            var putResponse = await _httpClient.PutAsJsonAsync("/products/1", new
            {
                ProductId = 1,
                Name = newName
            });

            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);

            var dbEntry = await _productRepository.Set<Domain.Entities.Product>()
                .FirstOrDefaultAsync(product =>
                    product.Name == newName
                );

            Assert.NotNull(dbEntry);
        }
    }
}