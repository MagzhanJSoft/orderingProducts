using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Products.Command;
using Order.Application.Products.DTOs;
using Order.Application.Products.Queries;

namespace Order.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator) => _mediator = mediator;

        [HttpPost("create")] 
        public async Task<IActionResult> Create(CreateProductCommand product)
        {
            if (product.AreEmptyName())            
                return BadRequest("Наименование продукта не должно быть пустым");

            if (!product.QuantityCheck())
                return BadRequest("Количество не должно быть отрицательным");

            var resultCreate = await _mediator.Send(product);
            if (!resultCreate.IsSuccess)
                return BadRequest(resultCreate.Error);

            return Ok(resultCreate.Value);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateProductCommand product)
        {
            if (product.AreEmptyName())
                return BadRequest("Наименование продукта не должно быть пустым");

            if (!product.QuantityCheck())
                return BadRequest("Количество не должно быть отрицательным");

            if (product.Id != id)
            {
                return BadRequest("Id не совпадают");
            }

            var resultUpdate = await _mediator.Send(product);
            if (!resultUpdate.IsSuccess)
                return BadRequest(resultUpdate.Error);

            return Ok(resultUpdate.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var resultDelete = await _mediator.Send(new DeleteProductsCommand(id));
            if (!resultDelete.IsSuccess)
                return BadRequest(resultDelete.Error);

            return Ok(resultDelete.Value);
        }

        [HttpGet] 
        public async Task<IActionResult> GetAll()
        {
           return Ok(await _mediator.Send(new GetProductsQuery()));
        }

        [HttpGet("searchbyname")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            var resultSearch = await _mediator.Send(new GetProductsByNameQuery(name));
            if (!resultSearch.IsSuccess)
                return BadRequest(resultSearch.Error);
            
            return Ok(resultSearch.Value);
        }
    }
}
