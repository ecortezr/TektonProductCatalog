using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Product.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // base.ConfigureWebHost(builder);

            builder.UseEnvironment("Testing");
            builder.ConfigureTestServices(services =>
            {
                services.AddDbContextFactory<ProductDbContext>(
                    o => o.UseInMemoryDatabase("products")
                );
            });
        }
    }
}
 