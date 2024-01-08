namespace Product.Api.Domain.Repositories
{
    public interface IDiscountClient
    {
        Task<HttpResponseMessage> GetRandomDiscountById(int id);
        Task<int> GetDiscountFromResult(HttpResponseMessage response);
    }
}
