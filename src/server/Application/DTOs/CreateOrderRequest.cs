using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateOrderRequest
    {
        public int DocumentTypeId { get; set; }
        public string Purpose { get; set; }
        public DocumentFormat Format { get; set; }
    }
}
