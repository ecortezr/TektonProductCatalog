using Confluent.Kafka;

namespace Product.Api.Extensions
{
    public static class KafkaExtensions
	{
        public static void AddKafka(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var bootstrapServer = $"{configuration["KafkaConfiguration:BootstrapServer"]}";
            var clientId = $"{configuration["KafkaConfiguration:ClientId"]}";
            var groupId = $"{configuration["KafkaConfiguration:GroupId"]}";

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServer,
                ClientId = clientId,
                BrokerAddressFamily = BrokerAddressFamily.V4,
            };
            services.AddSingleton<ProducerConfig>(producerConfig);

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = bootstrapServer,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                ClientId = clientId,
                GroupId = groupId,
                BrokerAddressFamily = BrokerAddressFamily.V4,
            };
            services.AddSingleton<ConsumerConfig>(consumerConfig);
        }
    }
}

