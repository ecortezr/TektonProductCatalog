using Microsoft.EntityFrameworkCore;

namespace Product.Api.Infrastructure.Storage
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Domain.Entities.Product> Products => Set<Domain.Entities.Product>();
    }
}
