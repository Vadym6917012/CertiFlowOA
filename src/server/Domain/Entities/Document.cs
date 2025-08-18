using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }

        [Required]
        public int DocumentTypeId {  get; set; }

        [ForeignKey(nameof(DocumentTypeId))]
        public DocumentType DocumentType { get; set; }

        [Required]
        public DocumentStatus Status {  get; set; } = DocumentStatus.New;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? SignedDate { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
