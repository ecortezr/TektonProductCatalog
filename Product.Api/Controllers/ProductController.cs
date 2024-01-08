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
        [HttpGet]
        [Route("[action]/{ProductId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int ProductId)
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
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Insert([FromBody] CreateProductCommand command)
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
        [HttpPut]
        [Route("[action]/{ProductId:int}")]
        public async Task<IActionResult> Update([FromRoute] int ProductId, [FromBody] UpdateBodyProductCommand command)
        {
            var fullCommand = new UpdateProductCommand()
            {
                ProductId = ProductId,
                Name = command.Name,
                Description = command.Description,
                Status = command.Status,
                Price = command.Price,
            };

            var updatedProduct = await _mediator.Send(fullCommand);

            return (updatedProduct is null)
                ? NotFound()
                : Ok(updatedProduct);
        }
    }
}