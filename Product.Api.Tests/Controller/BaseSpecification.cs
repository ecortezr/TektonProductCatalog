using Product.Api.Infrastructure;

namespace Product.Api.Tests.Controller
{
    public abstract class BaseSpecification
    {
        protected readonly ProductApiFactory _api;
        protected readonly HttpClient _httpClient;
        protected readonly ProductDbContext _dbContext;

        protected BaseSpecification()
        {
            _api = new ProductApiFactory();
            _httpClient = _api.CreateClient();
            _dbContext = _api.CreateProductDbContext();
        }
    }
}