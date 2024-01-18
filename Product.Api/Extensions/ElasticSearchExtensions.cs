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
            var url = $"{configuration["ELKConfiguration:Url"]}";
            var username = $"{configuration["ELKConfiguration:Username"]}";
            var password = $"{configuration["ELKConfiguration:Password"]}";
            var defaultIndex = $"{configuration["ELKConfiguration:Index"]}";

            var settings = new ConnectionSettings(new Uri(url))
                .BasicAuthentication(username, password)
                .PrettyJson()
                .EnableApiVersioningHeader()
                .DefaultIndex(defaultIndex);

            AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, defaultIndex);
        }

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings
                .DefaultMappingFor<Domain.Entities.Product>(m =>
                    m.IdProperty(p => p.ProductId)
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

