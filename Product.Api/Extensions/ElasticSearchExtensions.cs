using Nest;

namespace Product.Api.Extensions
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticsearch(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var url = $"{configuration["ELKConfiguration:url"]}";
            var username = $"{configuration["ELKConfiguration:username"]}";
            var password = $"{configuration["ELKConfiguration:password"]}";
            var defaultIndex = $"{configuration["ELKConfiguration:index"]}";

            var settings = new ConnectionSettings(new Uri(url)).BasicAuthentication(username, password)
                .PrettyJson()
                .DefaultIndex(defaultIndex);

            // AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, defaultIndex);
        }

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings
                .DefaultMappingFor<Domain.Entities.Product>(m => m
                    .Ignore(p => p.Price)
                );
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            client.Indices.Create(
                indexName,
                index => index.Map<Domain.Entities.Product>(x => x.AutoMap())
            );
        }
    }
}

