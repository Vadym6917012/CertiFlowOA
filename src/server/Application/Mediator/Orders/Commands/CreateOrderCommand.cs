using Domain.Entities;
using MediatR;

namespace Application.Mediator.Orders.Commands
{
    public class CreateOrderCommand : IRequest<int>
    {
        public string UserId { get; set; }
        public int DocumentTypeId { get; set; }
        public DocumentFormat Format { get; set; } = DocumentFormat.Electronic;
        public string Purpose { get; set; }
    }
}
