using System;
using Confluent.Kafka;
using Nest;

namespace Product.Api.Extensions
{
	public static class KafkaExtensions
	{
        public static void AddKafka(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            /*
            var url = $"{configuration["ELKConfiguration:url"]}";
            var username = $"{configuration["ELKConfiguration:username"]}";
            var password = $"{configuration["ELKConfiguration:password"]}";
            var defaultIndex = $"{configuration["ELKConfiguration:index"]}";
            */
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                ClientId = "my-app",
                GroupId = "my-group",
                BrokerAddressFamily = BrokerAddressFamily.V4,
            };
            services.AddSingleton<ConsumerConfig>(config);
        }
    }
}

