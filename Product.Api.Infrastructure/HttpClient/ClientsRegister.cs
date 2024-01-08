using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System.Text;
using System.Text.Json;

namespace Product.Infrastructure.HttpClient
{
    public static class ClientsRegister
    {
        private const int DEFAULT_RETRY_POLICY = 3;
        private static int _retryPolicy = 3;

        public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
        {
            var pollyRetryPolicy = configuration["PollyRetryPolicy"];
            _retryPolicy = pollyRetryPolicy is null
                ? DEFAULT_RETRY_POLICY
                : int.Parse(pollyRetryPolicy);

            AddMockApiServiceClient(services, configuration);

            return services;
        }

        public static IServiceCollection AddMockApiServiceClient(IServiceCollection services, IConfiguration configuration)
        {
            var mockApiUrl = configuration["MockapiUrl"];

            services.AddHttpClient("MockApiSvc", client =>
            {
                client.BaseAddress = new Uri($"{mockApiUrl}");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
                .AddTransientHttpErrorPolicy(x =>
                    x.WaitAndRetryAsync(_retryPolicy, _ => TimeSpan.FromMilliseconds(300))
                );

            return services;
        }

        public static StringContent CreateRequestMessage<T>(
            T content,
            Encoding? encoding = null,
            string mediaType = "application/json"
        )
        {
            return new StringContent(
                JsonSerializer.Serialize(
                    content,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    }
                ),
                encoding ?? Encoding.UTF8,
                mediaType
            );
        }
    }
}
