using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Product.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ProductDbContext _dbContext;

        public ProductController(ProductDbContext dbContext, ILogger<ProductController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        ///     Add a new product
        /// </summary>
        /// <param name="product"></param>
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [HttpPost(Name = "CreateProduct")]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            _logger.LogWarning("Getting current Fuel Rate value from Deparment of Energy");

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return Created("product", new Product() { Name = "Test 1", Price = 12 });
        }
    }
}