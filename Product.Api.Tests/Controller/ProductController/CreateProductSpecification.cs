using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;

namespace Product.Api.Tests.Controller.ProductController
{
    public class CreateProductSpecification : BaseSpecification
    {
        const string PRODUCT_NAME = "Test 1";

        [Fact]
        public async Task Should_Return_400_When_Send_Invalid_Product_Data()
        {

            var response = await _httpClient.PostAsJsonAsync("/Product/Insert", new
            {
                Stock = 0
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Should_Return_201_When_Send_Product()
        {

            var response = await _httpClient.PostAsJsonAsync("/Product/Insert", new
            {
                Name = PRODUCT_NAME,
                Description = "Test Description",
                Status = 1,
                Stock = 0,
                Price = 10
            });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Should_Add_To_Storage_When_Has_New_Product()
        {
            var response = await _httpClient.PostAsJsonAsync("/Product/Insert", new
            {
                Name = PRODUCT_NAME,
                Description = "Test Description",
                Status = 1,
                Stock = 0,
                Price = 10
            });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var dbEntry = await _dbContext.Products.FirstOrDefaultAsync(product =>
                product.Name == PRODUCT_NAME
            );

            Assert.NotNull(dbEntry);
        }
    }
}