using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Api.Domain.Enums;
using Product.Api.Infrastructure;

namespace Product.Api.Features.Products.Queries
{
    public class GetProductQuery : IRequest<GetProductQueryResponse>
    {
        public int ProductId { get; set; }

        public GetProductQuery(int productId)
        {
            ProductId = productId;
        }
    }

    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, GetProductQueryResponse>
    {
        private readonly ProductDbContext _context;
        private readonly IAppCache _cache;

        public GetProductQueryHandler(ProductDbContext context, IAppCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<GetProductQueryResponse?> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.ProductId == request.ProductId, cancellationToken);
            if (product is null)
            {
                return null;
            }

            var discount = GetDiscount(product.ProductId);

            var responseProduct = new GetProductQueryResponse()
            {
                ProductId = product.ProductId,
                Name = product.Name,
                StatusName = GetStatusName(product.Status),
                Stock = product.Stock,
                Description = product.Description,
                Price = product.Price,
                Discount = discount,
                FinalPrice = product.Price * (100 - discount) / 100
            };

            return responseProduct;
        }

        private string GetStatusName(ProductStatus status)
        {
            var defaultStatusName = "Unknown";

            var cachedStatus = _cache.Get<Dictionary<int, string>>("product-status");
            if (cachedStatus is null)
                return defaultStatusName;

            cachedStatus.TryGetValue((int)status, out string? statusName);

            return statusName ?? defaultStatusName;
        }

        private int GetDiscount(int id)
        {
            return id;
        }
    }

    public class GetProductQueryResponse
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string StatusName { get; set; } = default!;
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
