using CryptoInvestor.Infrastructure.Commands.Auth;

namespace CryptoInvestor.Infrastructure.Commands.Portfolio
{
    public class CreatePortfolio : AuthenticatedCommandBase
    {
        public string Name { get; set; }
    }
}