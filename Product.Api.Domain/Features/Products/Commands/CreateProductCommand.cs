using MediatR;
using Product.Api.Domain.Enums;
using Product.Api.Domain.Repositories;

namespace Product.Api.Domain.Features.Products.Commands
{
    public class CreateProductCommand : IRequest<Entities.Product>
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int Status { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Entities.Product>
    {
        private readonly IProductRepository _context;

        public CreateProductCommandHandler(IProductRepository context)
        {
            _context = context;
        }

        public async Task<Entities.Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var newProduct = new Entities.Product
            {
                Name = request.Name,
                Description = request.Description,
                Status = (request.Status == 1) ? ProductStatus.Active : ProductStatus.Inactive,
                Stock = request.Stock,
                Price = request.Price
            };

            var entity = _context.Set<Entities.Product>();
            entity.Add(newProduct);

            await _context.SaveChangesAsync(cancellationToken);

            return newProduct;
        }
    }
}
