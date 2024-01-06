using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Api.Domain.Enums;
using Product.Api.Infrastructure;

namespace Product.Api.Features.Products.Queries
{
    public class GetProductsQuery : IRequest<List<GetProductsQueryResponse>>
    {
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<GetProductsQueryResponse>>
    {
        private readonly ProductDbContext _context;

        public GetProductsQueryHandler(ProductDbContext context)
        {
            _context = context;
        }

        public Task<List<GetProductsQueryResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken) =>
            _context.Products
                .AsNoTracking()
                .Select(s => new GetProductsQueryResponse
                {
                    ProductId = s.ProductId,
                    Name = s.Name,
                    Description = s.Description,
                    Status = s.Status.ToString(),
                    Stock = s.Stock,
                    Price = s.Price
                })
                .ToListAsync(cancellationToken);
    }

    public class GetProductsQueryResponse
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string Status { get; set; } = default!;
        public int Stock { get; set; }
        public decimal Price { get; set; }
    }
}
