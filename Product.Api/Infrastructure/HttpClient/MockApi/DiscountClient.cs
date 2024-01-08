using Newtonsoft.Json;
using Product.Api.Exceptions;

namespace Product.Api.Infrastructure.HttpClient.MockApi
{
    public class DiscountClient : IDiscountClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string API_VERSION = "api/v1";

        public DiscountClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> GetRandomDiscountById(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("MockApiSvc");

            var fullUri = new Uri($"{httpClient.BaseAddress}/{API_VERSION}/discount/{id}");

            return await httpClient.GetAsync(fullUri);
        }

        public async Task<int> GetDiscountFromResult(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new InternalServerErrorException("Fail API call to MockApi");

            var content = await response.Content.ReadAsStringAsync();
            var serializedData = JsonConvert.DeserializeObject<MockApiResponse>(content);

            if (serializedData is null)
                throw new InternalServerErrorException("Fail API call to MockApi");

            return int.Parse(serializedData.discount);
        }
    }

    public class MockApiResponse
    {
        public string discount { get; set; } = default!;
        public string id { get; set; } = default!;
    }
}
