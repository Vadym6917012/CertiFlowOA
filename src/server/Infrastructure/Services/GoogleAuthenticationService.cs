using Application.Common.Interfaces;
using Domain.Entities.GoogleOAuth;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class GoogleAuthenticationService : IGoogleAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ILogger<GoogleAuthenticationService> _logger;

        public GoogleAuthenticationService(
            IConfiguration configuration, 
            HttpClient httpClient,
            ILogger<GoogleAuthenticationService> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            ValidateConfiguration();
        }

        private void ValidateConfiguration()
        {
            if ( string.IsNullOrEmpty(_configuration ["Google:ClientId"]) )
                throw new InvalidOperationException("Google ClientId is not configured");

            if ( string.IsNullOrEmpty(_configuration ["Google:ClientSecret"]) )
                throw new InvalidOperationException("Google ClientSecret is not configured");

            if ( string.IsNullOrEmpty(_configuration ["Google:RedirectUri"]) )
                throw new InvalidOperationException("Google RedirectUri is not configured");
        }

        /// <summary>
        /// Exchanges authorization code for access token
        /// </summary>
        public async Task<GoogleTokenResponse> ExchangeCodeOnTokenAsync(string code, string codeVerifier)
        {
            try
            {
                if ( string.IsNullOrEmpty(code))
                    throw new ArgumentException("Code cannot be null or empty", nameof(code));
                if ( string.IsNullOrEmpty(codeVerifier) )
                    throw new ArgumentException("Code verifier cannot be null or empty", nameof(codeVerifier));

                var tokenRequest = new Dictionary<string, string>
            {
                {"client_id", _configuration["Google:ClientId"]},
                {"client_secret", _configuration["Google:ClientSecret"]},
                {"code", code},
                {"code_verifier", codeVerifier},
                {"redirect_uri", _configuration["Google:RedirectUri"]},
                {"grant_type", "authorization_code"}
            };

                var response = await _httpClient.PostAsync(
                    "https://oauth2.googleapis.com/token", 
                    new FormUrlEncodedContent(tokenRequest));

                if ( !response.IsSuccessStatusCode )
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Google token exchange failed: {StatusCode} - {Content}",
                        response.StatusCode, errorContent);

                    throw response.StatusCode switch
                    {
                        HttpStatusCode.BadRequest => new GoogleAuthException("Invalid request parameters"),
                        HttpStatusCode.Unauthorized => new GoogleAuthException("Invalid client credentials"),
                        _ => new GoogleAuthException($"Token exchange failed: {response.StatusCode}")
                    };
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<GoogleTokenResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch ( Exception ex)
            {
                _logger.LogError(ex, "Error exchanging Google code for token");
                throw;
            }
        }

        /// <summary>
        /// Refreshes the access token using refresh token
        /// </summary>
        public async Task<GoogleRefreshTokenRequest> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                if (string.IsNullOrEmpty(refreshToken))
                    throw new ArgumentException("Refresh token cannot be null or empty", nameof(refreshToken));

                var tokenRequest = new Dictionary<string, string>
            {
                {"client_id", _configuration["Google:ClientId"]},
                {"client_secret", _configuration["Google:ClientSecret"]},
                {"refresh_token", refreshToken},
                {"grant_type", "refresh_token"},
            };

                var response = await _httpClient.PostAsync(
                    "https://oauth2.googleapis.com/token", 
                    new FormUrlEncodedContent(tokenRequest));

                if (!response.IsSuccessStatusCode )
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Google token refresh failed: {StatusCode} - {Content}",
                        response.StatusCode, errorContent);

                    throw new GoogleAuthException($"Token refresh failed: {response.StatusCode}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<GoogleRefreshTokenRequest>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch ( Exception ex)
            {
                _logger.LogError(ex, "Error refreshing Google token");
                throw;
            }
        }

        /// <summary>
        /// Gets user info from Google using access token
        /// </summary>
        public async Task<GoogleUserInfo> GetGoogleUserInfoAsync(string accessToken)
        {
            try
            {
                if ( string.IsNullOrEmpty(accessToken) )
                    throw new ArgumentException("Access token cannot be null or empty", nameof(accessToken));

                var response = await _httpClient.GetAsync(
                    $"https://www.googleapis.com/oauth2/v3/userinfo?access_token={accessToken}");

                if ( !response.IsSuccessStatusCode )
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Google user info fetch failed: {StatusCode} - {Content}",
                        response.StatusCode, errorContent);

                    throw new GoogleAuthException($"Failed to fetch user info: {response.StatusCode}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<GoogleUserInfo>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch ( Exception ex)
            {
                _logger.LogError(ex, "Error fetching Google user info");
                throw;
            }
        }
    }
}
