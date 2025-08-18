using Application.Common.Interfaces;
using Domain.Constants;
using Domain.Entities;
using Domain.Entities.JWT;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<ApplicationUser> _userRepository;
        private readonly IGenericRepository<JwtRefreshTokens> _jwtRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IGenericRepository<ApplicationUser> userRepository, IGenericRepository<JwtRefreshTokens> jwtRepository, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _jwtRepository = jwtRepository;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task AddUserAsync(ApplicationUser user)
        {
            var result = await _userManager.CreateAsync(user);

            await _userManager.AddToRoleAsync(user, SD.StudentRole);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error creating user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Error updating user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        public async Task AddRefreshTokenAsync(JwtRefreshTokens refreshToken)
        {
            await _jwtRepository.AddAsync(refreshToken);
        }

        // Метод для отримання RefreshToken по користувачу
        public async Task<JwtRefreshTokens> GetRefreshTokenByUserIdAsync(string userId)
        {
            return await _jwtRepository.FindAsync(rt => rt.UserId == userId);
        }

        // Метод для видалення старого RefreshToken (якщо необхідно)
        public async Task DeleteRefreshTokenAsync(JwtRefreshTokens refreshToken)
        {
            await _jwtRepository.DeleteAsync(refreshToken);
        }
    }
}
