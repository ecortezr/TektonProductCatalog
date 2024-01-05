using Microsoft.EntityFrameworkCore;

namespace Product.Api.Domain
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Product> Products => Set<Product>();
    }
}
