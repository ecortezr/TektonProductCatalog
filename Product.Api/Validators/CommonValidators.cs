using Microsoft.EntityFrameworkCore;
using Product.Api.Infrastructure;

namespace Product.Api.Validators
{
    public class CommonValidators
    {
        private readonly ProductDbContext _context;

        public CommonValidators(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsAValidProduct(int productId)
        {

            return await _context.Products
                .FirstOrDefaultAsync(x => x.ProductId == productId) != null;
        }
    }
}
