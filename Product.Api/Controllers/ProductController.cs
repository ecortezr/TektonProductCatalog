using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.Api.Features.Products.Commands;
using Product.Api.Features.Products.Queries;
using Product.Api.Infrastructure;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Product.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///     List all products
        /// </summary>
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [HttpGet(Name = "GetProducts")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetProductsQuery()));
        }

        /// <summary>
        ///     Get a product by Id
        /// </summary>
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [HttpGet("{ProductId:int}")]
        public async Task<IActionResult> Get([FromRoute] int ProductId)
        {
            var product = await _mediator.Send(new GetProductQuery(ProductId));
            return (product is null)
                ? NotFound()
                : Ok(product);
        }

        /// <summary>
        ///     Add a new product
        /// </summary>
        /// <param name="command"></param>
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [HttpPost(Name = "CreateProduct")]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            var newProduct = await _mediator.Send(command);

            return Created("product", newProduct);
        }

        /// <summary>
        ///     Update a product
        /// </summary>
        /// <param name="command"></param>
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [HttpPut(Name = "UpdateProduct")]
        public async Task<IActionResult> Update(UpdateProductCommand command)
        {
            var updatedProduct = await _mediator.Send(command);

            return (updatedProduct is null)
                ? NotFound()
                : Ok(updatedProduct);
        }
    }
}