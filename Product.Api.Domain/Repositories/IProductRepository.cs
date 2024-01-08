using Microsoft.EntityFrameworkCore;

namespace Product.Api.Domain.Repositories
{
    public interface IProductRepository
    {
        DbSet<T> Set<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
