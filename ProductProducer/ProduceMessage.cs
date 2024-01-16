using Confluent.Kafka;

namespace ProductProducer
{
    public class ProduceMessage
	{
        public async Task CreateMessage()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = "my-app",
                BrokerAddressFamily = BrokerAddressFamily.V4,
            };
            using var producer = new ProducerBuilder<Null, string>(config)
                .Build();

            var input = "start";
            while (!string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Please enter the message you want to send: ");
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    continue;

                var message = new Message<Null, string>
                {
                    Value = input
                };

                var deliveryReport = await producer.ProduceAsync("my-basic-topic", message);
                Console.WriteLine($"Message '{input}' delivered to {deliveryReport.TopicPartitionOffset}");
            }
            Console.WriteLine("Productor ended!");
        }
    }
}

