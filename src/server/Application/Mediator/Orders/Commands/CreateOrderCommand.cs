using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
