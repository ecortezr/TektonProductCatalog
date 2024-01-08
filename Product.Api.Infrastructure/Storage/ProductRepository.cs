using Microsoft.EntityFrameworkCore;
using Product.Api.Domain.Repositories;

namespace Product.Api.Infrastructure.Storage
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Set<T>() where T : class
        {
            return _context.Set<T>();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
