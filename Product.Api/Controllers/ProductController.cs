using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Api.Features.Products.Commands;
using Product.Api.Features.Products.Queries;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

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
        ///     Get a product by Id
        /// </summary>
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{ProductId:int}")]
        public async Task<IActionResult> Get([FromRoute] int ProductId)
        {
            GetProductQueryResponse product;
            try
            {
                product = await _mediator.Send(new GetProductQuery(ProductId));
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

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