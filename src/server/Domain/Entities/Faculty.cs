using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Faculty
    {
        [Key]
        public int FacultyId { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}