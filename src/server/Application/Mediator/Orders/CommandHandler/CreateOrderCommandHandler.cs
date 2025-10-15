using Application.Common.Interfaces;
using Application.Mediator.Orders.Commands;
using Domain.Entities;
using MediatR;

namespace Application.Mediator.Orders.CommandHandler
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<Document> _docRepo;
        private readonly IGenericRepository<DocumentType> _docTypeRepo;

        public CreateOrderCommandHandler(
            IGenericRepository<Order> orderRepo, 
            IGenericRepository<Document> docRepo,
            IGenericRepository<DocumentType> docTypeRepo)
        {
            _orderRepo = orderRepo;
            _docRepo = docRepo;
            _docTypeRepo = docTypeRepo;
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            if ( !Enum.IsDefined(typeof(DocumentFormat), request.Format) )
                throw new ArgumentException($"Невірний формат документа: {request.Format}");

            var documentType = await _docTypeRepo.GetByIdAsync(request.DocumentTypeId, cancellationToken);
            if(documentType == null)
            {
                throw new ArgumentException($"Тип документа з ID: {request.DocumentTypeId} не знайдено.");
            }

            var document = new Document
            {
                DocumentTypeId = request.DocumentTypeId,
                DocumentType = documentType,
                Status = DocumentStatus.New,
                CreatedDate = DateTime.UtcNow
            };

            await _docRepo.AddAsync(document);

            // Створюємо замовлення
            var order = new Order
            {
                UserId = request.UserId,
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
