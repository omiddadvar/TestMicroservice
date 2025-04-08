using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Commands;
using OrderService.Application.DTOs;

namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAPI([FromBody] CreateOrder.Command orderCommand 
            , CancellationToken cancellationToken = default)
        {
            OrderDTO result = await _mediator.Send(orderCommand, cancellationToken);
            return Ok(result);
        }
    }
}
