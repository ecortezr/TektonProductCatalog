using Product.Api.Infrastructure;
using System.Net;

namespace Product.Api.Tests.Controller
{
    public abstract class BaseSpecification
    {
        protected readonly ProductApiFactory _api;
        protected readonly HttpClient _httpClient;
        protected readonly ProductDbContext _dbContext;

        public BaseSpecification()
        {
            _api = new ProductApiFactory();
            _httpClient = _api.CreateClient();
            _dbContext = _api.CreateProductDbContext();
        }
    }
}