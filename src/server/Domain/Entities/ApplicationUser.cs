using Domain.Entities.GoogleOAuth;
using Domain.Entities.JWT;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }

        public int FacultyId { get; set; }
        public string? PictureUrl { get; set; }


        [ForeignKey("FacultyId")]
        public virtual Faculty? Faculty { get; set; }

        public virtual GoogleToken? GoogleToken { get; set; }


        public virtual ICollection<JwtRefreshTokens> RefreshTokens { get; set; } = new List<JwtRefreshTokens>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public void RevokeRefreshToken(string token, string ipAddress)
        {
            var refreshToken = RefreshTokens.FirstOrDefault(x => x.Token == token);
            if ( refreshToken == null ) return;

            refreshToken.RevokedAt = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
        }
    }
}
