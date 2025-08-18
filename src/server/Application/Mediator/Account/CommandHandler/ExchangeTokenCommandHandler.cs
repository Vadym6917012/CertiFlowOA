using Application.Common.Interfaces;
using Application.DTOs.Account;
using Application.Mediator.Account.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.GoogleOAuth;
using MediatR;

namespace Application.Mediator.Account.CommandHandler
{
    public class ExchangeTokenCommandHandler : IRequestHandler<ExchangeTokenCommand, UserTokenDto>
    {
        private readonly IGoogleAuthenticationService _googleAuthenticationService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IJWTService _jwtService;
        private readonly IGenericRepository<GoogleToken> _googleTokenRepository;

        public ExchangeTokenCommandHandler(IGoogleAuthenticationService googleAuthenticationService,
            IUserService userService,
            IMapper mapper,
            IJWTService jwtService,
            IGenericRepository<GoogleToken> googleTokenRepository)
        {
            _googleAuthenticationService = googleAuthenticationService;
            _userService = userService;
            _mapper = mapper;
            _jwtService = jwtService;
            _googleTokenRepository = googleTokenRepository;
        }

        public async Task<UserTokenDto> Handle(ExchangeTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenResponse = await _googleAuthenticationService.ExchangeCodeOnTokenAsync(request.AuthorizationCode, request.CodeVerifier);
            var googleUserInfo = await _googleAuthenticationService.GetGoogleUserInfoAsync(tokenResponse.AccessToken);

            var existingUser = await _userService.FindByEmailAsync(googleUserInfo.Email);

            if (existingUser != null)
            {
                await UpdateGoogleTokenAsync(existingUser.Id, tokenResponse, cancellationToken);

                if (existingUser.PictureUrl != googleUserInfo.Picture)
                {
                    existingUser.PictureUrl = googleUserInfo.Picture;

                    await _userService.UpdateUserAsync(existingUser);
                }
            }
            else
            {
                var newUser = new ApplicationUser
                {
                    Name = googleUserInfo.Name,
                    UserName = googleUserInfo.Email,
                    NormalizedUserName = googleUserInfo.Email.ToUpper(),
                    Email = googleUserInfo.Email,
                    NormalizedEmail = googleUserInfo.Email.ToUpper(),
                    EmailConfirmed = googleUserInfo.emailVerified,
                    PictureUrl = googleUserInfo.Picture,
                    FacultyId = 1
                };

                await _userService.AddUserAsync(newUser);

                await SaveGoogleTokenAsync(newUser.Id, tokenResponse, cancellationToken);

                existingUser = newUser;
            }

            var jwtAccessToken = await _jwtService.CreateJWT(existingUser);

            var newRefreshToken = _jwtService.CreateRefreshToken(existingUser);
            await _userService.AddRefreshTokenAsync(newRefreshToken);

            var userTokenDto = new UserTokenDto
            {
                AccessToken = jwtAccessToken,
                Name = googleUserInfo.Name,
                Email = googleUserInfo.Email,
                PictureUrl = googleUserInfo.Picture
            };

            return userTokenDto;
        }

        private async Task UpdateGoogleTokenAsync(string userId, GoogleTokenResponse googleTokenResponse, CancellationToken cancellationToken)
        {
            var existingToken = await _googleTokenRepository.FindAsync(t => t.UserId == userId, cancellationToken);

            if (existingToken == null)
            {
                await SaveGoogleTokenAsync(userId, googleTokenResponse, cancellationToken);
            } else
            {
                existingToken.AccessToken = googleTokenResponse.AccessToken;
                existingToken.RefreshToken = googleTokenResponse.RefreshToken;
                existingToken.ExpiresAt = DateTime.UtcNow.AddSeconds(googleTokenResponse.ExpiresIn);
                await _googleTokenRepository.UpdateAsync(existingToken, cancellationToken);
            }
        }

        private async Task SaveGoogleTokenAsync(string userId, GoogleTokenResponse tokenResponse, CancellationToken cancellationToken)
        {
            var newToken = new GoogleToken
            {
                UserId = userId,
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
                ExpiresAt = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn)
            };

            await _googleTokenRepository.AddAsync(newToken, cancellationToken);
        }
    }
}
