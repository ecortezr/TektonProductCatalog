using Confluent.Kafka;
using Nest;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry.Serdes;

namespace Product.Api.HostedServices
{
    public class KafKaProductTopicConsumer : BackgroundService
    {
        private readonly IElasticClient _elasticClient;
        private readonly ConsumerConfig _consumerConfig;
        private readonly IConfiguration _configuration;

        public KafKaProductTopicConsumer(
            IElasticClient elasticClient,
            ConsumerConfig consumerConfig,
            IConfiguration configuration
        )
        {
            _elasticClient = elasticClient;
            _consumerConfig = consumerConfig;
            _configuration = configuration;
        }


        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("KafKa Product Topic Consumer is starting...");

            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("KafKa Product Topic Consumer is stopping...");

            await base.StopAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() => StartConsumerAsync(stoppingToken), stoppingToken);
            return Task.CompletedTask;
        }

        private async Task StartConsumerAsync(CancellationToken stoppingToken)
        {
            try
            {
                Console.WriteLine($"StartConsumerAsync method started!");

                var productTopic = $"{_configuration["KafkaConfiguration:ProductTopic"]}";
                //var deseralizer = new JsonDeserializer
                using var consumer = new ConsumerBuilder<Ignore, Domain.Entities.Product>(_consumerConfig)
                    .SetKeyDeserializer(Deserializers.Ignore)
                    .SetValueDeserializer(new JsonDeserializer<Domain.Entities.Product>().AsSyncOverAsync())
                    .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                    .Build();
                consumer.Subscribe(productTopic);
                Console.WriteLine($"StartConsumerAsync Consumer subscribe to {productTopic}");

                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    var consumedMessage = consumeResult.Message.Value;

                    Console.WriteLine($"Message received from {consumeResult.TopicPartitionOffset}: {consumedMessage}");

                    if (consumedMessage is not null)
                    {
                        Console.WriteLine($"Product details: {{ Id: {consumedMessage.ProductId}, Name: '{consumedMessage.Name}' }}");

                        // Add product to Elastic Search
                        var indexResponse = await _elasticClient.IndexDocumentAsync(consumedMessage, stoppingToken);
                        if (!indexResponse.IsValid)
                        {
                            Console.WriteLine($"debugInfo: {indexResponse.DebugInformation}");
                            Console.WriteLine($"error: {indexResponse.ServerError.Error}");
                        }
                    }
                }

                // consumer.Close();

                Console.WriteLine($"StartConsumerAsync method ended!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
        }
    }
}

