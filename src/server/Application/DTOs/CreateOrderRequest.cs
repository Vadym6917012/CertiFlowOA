using Domain.Entities;

namespace Application.DTOs
{
    public class CreateOrderRequest
    {
        public int DocumentTypeId { get; set; }
        public DocumentFormat Format { get; set; }
        public string Purpose { get; set; }
    }
}
