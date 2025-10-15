using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int DocumentId { get; set; }
        public string DocumentTypeName { get; set; }
        public string DocumentStatus { get; set; }
        public string Purpose { get; set; }
        public string Format { get; set; } 
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}
