using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.Api.Infrastructure;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading;

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
        ///     Get products
        /// </summary>
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [HttpGet(Name = "GetProducts")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _dbContext.Products.ToListAsync());
        }

        /// <summary>
        ///     Add a new product
        /// </summary>
        /// <param name="product"></param>
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [HttpPost(Name = "CreateProduct")]
        public async Task<IActionResult> Create([FromBody] Domain.Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return Created("product", product);
        }
    }
}