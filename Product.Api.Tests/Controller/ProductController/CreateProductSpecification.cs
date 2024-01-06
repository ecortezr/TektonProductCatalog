using Microsoft.EntityFrameworkCore;
using Product.Api.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace Product.Api.Tests.Controller.ProductController
{
    public class CreateProductSpecification
    {
        private readonly ProductApiFactory _api;
        private readonly HttpClient _httpClient;
        private readonly ProductDbContext _dbContext;
        const string PRODUCT_NAME = "Test 1";

        public CreateProductSpecification()
        {
            _api = new ProductApiFactory();
            _httpClient = _api.CreateClient();
            _dbContext = _api.CreateProductDbContext();
        }

        [Fact]
        public async Task Should_Return_400_When_Send_Invalid_Product()
        {

            var response = await _httpClient.PostAsJsonAsync("/Product", new
            {
                Stock = 0
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Should_Return_201_When_Send_Product()
        {

            var response = await _httpClient.PostAsJsonAsync("/Product", new
            {
                Name = PRODUCT_NAME,
                Price = 12
            });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Should_Add_To_Storage_When_Has_New_Product()
        {
            var response = await _httpClient.PostAsJsonAsync("/Product", new
            {
                Name = PRODUCT_NAME,
                Price = 12
            });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var dbEntry = await _dbContext.Products.FirstOrDefaultAsync(product =>
                product.Name == PRODUCT_NAME
            );

            Assert.NotNull(dbEntry);
        }
    }
}