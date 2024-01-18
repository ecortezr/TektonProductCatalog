using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using MediatR;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        private readonly IProductRepository _context;
        private readonly ProducerConfig _producerConfig;

        public CreateProductCommandHandler(
            IConfiguration configuration,
            IProductRepository context,
            ProducerConfig producerConfig
        )
        {
            _configuration = configuration;
            _context = context;
            _producerConfig = producerConfig;
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

            ProduceKafkaProductMessage(newProduct, cancellationToken);

            return newProduct;
        }

        private void ProduceKafkaProductMessage(Entities.Product product, CancellationToken cancellationToken)
        {
            var productTopic = $"{_configuration["KafkaConfiguration:ProductTopic"]}";
            var schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = "http://localhost:8081"
            };
            using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
            using var producer = new ProducerBuilder<Null, Entities.Product>(_producerConfig)
                .SetValueSerializer(new JsonSerializer<Entities.Product>(schemaRegistry))
                .Build();
            var message = new Message<Null, Entities.Product>
            {
                Value = product
            };

            var deliveryReport = producer.ProduceAsync(productTopic, message, cancellationToken);
            Console.WriteLine($"Message with ProductId '{product.ProductId}' delivered to {deliveryReport.Result.TopicPartitionOffset}");
        }
    }
}
