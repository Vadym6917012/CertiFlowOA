using Application.DTOs;
using MediatR;

namespace Application.Mediator.Orders.Queries
{
    public class GetOrdersQuery : IRequest<List<OrderDto>>
    {
        public string UserId { get; set; }
    }
}
