using Application.Common.Interfaces;
using Application.Mediator.Orders.Commands;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mediator.Orders.CommandHandler
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<Document> _docRepo;

        public CreateOrderCommandHandler(IGenericRepository<Order> orderRepo, IGenericRepository<Document> docRepo)
        {
            _orderRepo = orderRepo;
            _docRepo = docRepo;
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var document = new Document
            {
                DocumentTypeId = request.DocumentTypeId,
                CreatedDate = DateTime.UtcNow
            };

            await _docRepo.AddAsync(document);

            // Створюємо замовлення
            var order = new Order
            {
                UserId = request.UserId,
                DocumentId = document.DocumentId, // <- беремо ID створеного документа
                Document = document,
                Purpose = request.Purpose,
                Format = request.Format,
                CreatedDate = DateTime.UtcNow
            };

            await _orderRepo.AddAsync(order);

            return order.OrderId;
        }
    }
}
