using Microsoft.EntityFrameworkCore;

namespace Product.Api.Infrastructure
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Domain.Product> Products => Set<Domain.Product>();
    }
}
