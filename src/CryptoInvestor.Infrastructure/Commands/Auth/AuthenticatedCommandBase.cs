using System;

namespace CryptoInvestor.Infrastructure.Commands.Auth
{
    public class AuthenticatedCommandBase : IAuthenticatedCommand
    {
        public Guid UserId { get; set; }
    }
}