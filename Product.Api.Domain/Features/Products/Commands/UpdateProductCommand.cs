using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Api.Domain.Enums;
using Product.Api.Domain.Repositories;

namespace Product.Api.Domain.Features.Products.Commands
{
    public class UpdateBodyProductCommand : IRequest<Entities.Product>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public decimal? Price { get; set; }
    }

    public class UpdateProductCommand : UpdateBodyProductCommand
    {
        public int ProductId { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Entities.Product>
    {
        private readonly IProductRepository _context;

        public UpdateProductCommandHandler(IProductRepository context)
        {
            _context = context;
        }

        public async Task<Entities.Product?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Set<Entities.Product>()
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
