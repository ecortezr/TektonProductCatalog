﻿using MediatR;
using Product.Api.Domain.Enums;
using Product.Api.Infrastructure;

namespace Product.Api.Features.Products.Commands
{
    public class CreateProductCommand : IRequest<Domain.Entities.Product>
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int Status { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Domain.Entities.Product>
    {
        private readonly ILogger<CreateProductCommandHandler> _logger;
        private readonly ProductDbContext _context;

        public CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger, ProductDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<Domain.Entities.Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var newProduct = new Domain.Entities.Product
            {
                Name = request.Name,
                Description = request.Description,
                Status = (request.Status == 1) ? ProductStatus.Active : ProductStatus.Inactive,
                Stock = request.Stock,
                Price = request.Price
            };

            _context.Products.Add(newProduct);

            await _context.SaveChangesAsync(cancellationToken);

            return newProduct;
        }
    }
}