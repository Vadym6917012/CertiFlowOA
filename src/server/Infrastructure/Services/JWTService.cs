using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.JWT;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _cfg;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SymmetricSecurityKey _jwtKey;
        private readonly ILogger<JWTService> _logger;

        public JWTService(
            IConfiguration cfg,
            UserManager<ApplicationUser> userManager,
            ILogger<JWTService> logger)
        {
            _cfg = cfg ?? throw new ArgumentNullException(nameof(cfg));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var jwtKey = _cfg["JWT:Key"];
            if ( string.IsNullOrEmpty(jwtKey) )
            {
                throw new InvalidOperationException("JWT key is not configured");
            }
            _jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        }

        /// <summary>
        /// Creates a JWT token for the specified user
        /// </summary>
        public async Task<string> CreateJWT(ApplicationUser user)
        {
            try
            {
                if (user == null )
                {
                    throw new ArgumentNullException(nameof(user));
                }

                var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.Name),
            };

                var roles = await _userManager.GetRolesAsync(user);
                userClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var credentials = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha256Signature);

                var expiresInDays = int.TryParse(_cfg["JWT:ExpiresInDays"], out var days) ? days : 7;

                var issuer = _cfg["JWT:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured");

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(userClaims),
                    Expires = DateTime.UtcNow.AddDays(expiresInDays),
                    SigningCredentials = credentials,
                    Issuer = issuer
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwt = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(jwt);
            }
            catch ( Exception ex)
            {
                _logger.LogError(ex, "Error creating JWT token for user {UserId}", user?.Id);

                throw;
            }
        }

        /// <summary>
        /// Creates a refresh token for the specified user
        /// </summary>
        public JwtRefreshTokens CreateRefreshToken(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var token = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(token);

            var expiresInDays = int.TryParse(_cfg ["JWT:RefreshTokenExpiresInDays"] ?? "14", out var days) ? days : 14;

            return new JwtRefreshTokens()
            {
                Token = Convert.ToBase64String(token),
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(expiresInDays)
            };
        }

        /// <summary>
        /// Gets the token validation parameters
        /// </summary>
        public TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _jwtKey,
                ValidateIssuer = true,
                ValidIssuer = _cfg["JWT:Issuer"],
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // No clock skew for token expiration
            };
        }
    }
}
