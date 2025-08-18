using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.GoogleOAuth
{
    public class GoogleToken
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        [Required]
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
        [Required]
        [MaxLength(50)]
        public string TokenType { get; set; } = "Bearer";

        [MaxLength(500)]
        public string? Scope { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
