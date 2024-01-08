using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using Product.Api.Domain.Repositories;
using Product.Api.Infrastructure.HttpClient.MockApi;
using System.Net;

namespace Product.Api.Tests
{
    public class ProductApiFactory : WebApplicationFactory<IApiAssemblyMarker>
    {
        public ProductDbContext CreateProductDbContext() {
            var db = Services.GetRequiredService<IDbContextFactory<ProductDbContext>>().CreateDbContext();
            db.Database.EnsureCreated();

            return db;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment("Testing");
            builder.ConfigureTestServices(services =>
            {
                services.AddDbContextFactory<ProductDbContext>(
                    o => o.UseInMemoryDatabase("products")
                );

                services.AddScoped<IDiscountClient>(provider => GetDiscountClientClass());
            });
        }

        private static IDiscountClient GetDiscountClientClass()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{ 'discount': '10', 'id': '1' }")
                });

            var client = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://www.tekton.com/")
            };

            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(_ => _.CreateClient("MockApiSvc")).Returns(client);

            return new DiscountClient(mockFactory.Object);
        }
    }
}
 