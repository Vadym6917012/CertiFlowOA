using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class DocumentType
    {
        [Key]
        public int DocumentTypeId { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        public ICollection<Document> Documents { get; set; } = new List<Document>();
    }
}
