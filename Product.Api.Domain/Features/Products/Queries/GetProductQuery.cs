using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Api.Domain.Enums;
using Product.Api.Domain.Repositories;
using System.Security.Cryptography;

namespace Product.Api.Domain.Features.Products.Queries
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
        private readonly IProductRepository _context;
        private readonly IAppCache _cache;
        private readonly IDiscountClient _discountClient;

        public GetProductQueryHandler(
            IProductRepository context,
            IAppCache cache,
            IDiscountClient discountClient
        )
        {
            _context = context;
            _cache = cache;
            _discountClient = discountClient;
        }

        public async Task<GetProductQueryResponse?> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Set<Entities.Product>()
                .FirstOrDefaultAsync(x => x.ProductId == request.ProductId, cancellationToken);
            if (product is null)
            {
                return null;
            }

            var discount = await GetDiscount(product.ProductId);

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

        private async Task<int> GetDiscount(int productId)
        {
            var randomId = GetRandomIdForEndpoint(productId);
            var apiResponse = await _discountClient.GetRandomDiscountById(randomId);

            return await _discountClient.GetDiscountFromResult(apiResponse);
        }

        private int GetRandomIdForEndpoint(int productId, int lowerBound = 0, int upperBound = 101)
        {
            return RandomNumberGenerator.GetInt32(lowerBound, upperBound);
            //var random = new Random(productId);
            //return random.Next(lowerBound, upperBound);
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
