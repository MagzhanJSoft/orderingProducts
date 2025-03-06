using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Order.Command;
using Order.Application.Order.Queries;
using Order.Application.Products.Command;
using Order.Application.Products.Queries;

namespace Order.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrdersController(IMediator mediator) => _mediator = mediator;

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateOrderCommand order)
        {
            var resultCreate = await _mediator.Send(order);
            if (!resultCreate.IsSuccess)
                return BadRequest(resultCreate.Error);

            return Ok(resultCreate.Value);
        }

        [HttpGet("cancel/{id}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var resultCancel = await _mediator.Send(new CancelOrderCommand(id));
            if (!resultCancel.IsSuccess)
                return BadRequest(resultCancel.Error);

            return Ok(resultCancel.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new GetOrdersQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var resultSearch = await _mediator.Send(new GetOrdersByIdQuery(id));
            if (!resultSearch.IsSuccess)
                return BadRequest(resultSearch.Error);

            return Ok(resultSearch.Value);
        }
    }


}
