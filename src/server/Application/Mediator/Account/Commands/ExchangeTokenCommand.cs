using Application.DTOs.Account;
using MediatR;

namespace Application.Mediator.Account.Commands
{
    public class ExchangeTokenCommand : IRequest<UserTokenDto>
    {
        public string AuthorizationCode { get; set; }
        public string CodeVerifier { get; set; }
    }
}
