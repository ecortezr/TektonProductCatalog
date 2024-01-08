using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Api.Domain.Enums;
using Product.Api.Infrastructure;

namespace Product.Api.Features.Products.Commands
{
    public class UpdateProductCommand : IRequest<Domain.Entities.Product>
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public decimal? Price { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Domain.Entities.Product>
    {
        private readonly ProductDbContext _context;

        public UpdateProductCommandHandler(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.Product?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.ProductId == request.ProductId, cancellationToken);

            if (product is null)
            {
                return null;
            }

            if (request.Name != null) product.Name = request.Name;
            if (request.Description != null) product.Description = request.Description;
            if (request.Status != null) product.Status = (ProductStatus)request.Status;
            if (request.Price != null) product.Price = (decimal)request.Price;

            await _context.SaveChangesAsync(cancellationToken);

            return product;
        }
    }
}
