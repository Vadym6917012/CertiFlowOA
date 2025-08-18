using Domain.Entities;
using Domain.Entities.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IJWTService
    {
        Task<string> CreateJWT(ApplicationUser user);
        JwtRefreshTokens CreateRefreshToken(ApplicationUser user);
    }
}
