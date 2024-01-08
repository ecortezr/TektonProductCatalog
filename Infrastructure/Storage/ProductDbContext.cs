using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Product.Api.Infrastructure.Storage
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Domain.Entities.Product> Products { get; set; }
    }
}
