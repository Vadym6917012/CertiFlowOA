using Domain.Entities;
using Domain.Entities.JWT;

namespace Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task AddUserAsync(ApplicationUser user);
        Task UpdateUserAsync(ApplicationUser user);
        Task AddRefreshTokenAsync(JwtRefreshTokens refreshToken);
    }
}
