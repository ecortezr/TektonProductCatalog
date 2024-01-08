namespace Product.Api.Infrastructure.HttpClient
{
    public interface IDiscountClient
    {
        Task<HttpResponseMessage> GetRandomDiscountById(int id);
        Task<int> GetDiscountFromResult(HttpResponseMessage response);
    }
}
