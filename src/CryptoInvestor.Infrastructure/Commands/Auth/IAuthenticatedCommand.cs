using System;

namespace CryptoInvestor.Infrastructure.Commands.Auth
{
    public interface IAuthenticatedCommand : ICommand
    {
        Guid UserId { get; set; }
    }
}