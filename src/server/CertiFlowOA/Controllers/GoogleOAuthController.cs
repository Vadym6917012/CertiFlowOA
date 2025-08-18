using Microsoft.AspNetCore.Mvc;
using Application.Common.Interfaces;
using MediatR;
using Domain.Entities.GoogleOAuth;
using Application.Mediator.Account.Commands;

namespace CertiFlowOA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoogleOAuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IGoogleAuthenticationService _googleAuthenticationService;

        public GoogleOAuthController(IMediator mediator, IGoogleAuthenticationService googleAuthenticationService)
        {
            _mediator = mediator;
            _googleAuthenticationService = googleAuthenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> ExchangeTokenAsync([FromBody] GoogleTokenExchangeRequest request)
        {
            var command = new ExchangeTokenCommand
            {
                AuthorizationCode = request.AuthorizationCode,
                CodeVerifier = request.CodeVerifier,
            };

            var userTokenResponse = await _mediator.Send(command);

            return Ok(userTokenResponse);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] GoogleRefreshTokenRequest request)
        {
            var refreshTokenResponse = await _googleAuthenticationService.RefreshTokenAsync(request.RefreshToken);

            return Ok(refreshTokenResponse);
        }
    }
}
