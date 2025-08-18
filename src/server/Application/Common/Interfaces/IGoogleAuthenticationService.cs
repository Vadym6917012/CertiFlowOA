using Domain.Entities.GoogleOAuth;

namespace Application.Common.Interfaces
{
    public interface IGoogleAuthenticationService
    {
        Task<GoogleTokenResponse> ExchangeCodeOnTokenAsync(string code, string codeVerifier);
        Task<GoogleRefreshTokenRequest> RefreshTokenAsync(string refreshToken);
        Task<GoogleUserInfo> GetGoogleUserInfoAsync(string accessToken);
    }
}
