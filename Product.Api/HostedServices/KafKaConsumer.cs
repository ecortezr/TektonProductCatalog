using Confluent.Kafka;

namespace Product.Api.HostedServices
{
    public class KafKaConsumer : BackgroundService
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly IConfiguration _configuration;

        public KafKaConsumer(ConsumerConfig consumerConfig, IConfiguration configuration)
        {
            _consumerConfig = consumerConfig;
            _configuration = configuration;
        }


        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("KafKa Consumer is starting...");

            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("KafKa Consumer is stopping...");

            await base.StopAsync(cancellationToken);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() => StartConsumerAsync(stoppingToken), stoppingToken);
            return Task.CompletedTask;
            // await StartConsumer(stoppingToken);
        }

        private async Task StartConsumerAsync(CancellationToken stoppingToken)
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();
            consumer.Subscribe("my-basic-topic");

            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = consumer.Consume(stoppingToken);
                var consumedMessage = consumeResult.Message.Value;

                if (!string.IsNullOrEmpty(consumedMessage))
                {
                    Console.WriteLine($"Message received from {consumeResult.TopicPartitionOffset}: {consumedMessage}");
                }
            }

            consumer.Close();
        }
    }
}

