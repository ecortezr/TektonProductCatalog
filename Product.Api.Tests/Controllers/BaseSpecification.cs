using Product.Api.Domain.Repositories;
using Product.Api.Infrastructure.Storage;

namespace Product.Api.Tests.Controllers
{
    public abstract class BaseSpecification
    {
        protected readonly ProductApiFactory _api;
        protected readonly HttpClient _httpClient;
        protected readonly IProductRepository _productRepository;

        protected BaseSpecification()
        {
            _api = new ProductApiFactory();
            _httpClient = _api.CreateClient();
            _productRepository = new ProductRepository(_api.CreateProductDbContext());
        }
    }
}