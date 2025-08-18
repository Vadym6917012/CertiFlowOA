using Application.DTOs;
using Application.Mediator.Orders.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CertiFlowOA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var command = new CreateOrderCommand
            {
                UserId = userId,
                DocumentTypeId = request.DocumentTypeId,
                Format = request.Format,
                Purpose = request.Purpose
            };

            var orderId = await _mediator.Send(command);
            return Ok(new { orderId });
        }
    }
}
