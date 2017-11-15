using CryptoInvestor.Infrastructure.Commands.Auth;

namespace CryptoInvestor.Infrastructure.Commands.Portfolio
{
    public class UpdatePortfolio : AuthenticatedCommandBase
    {
        public string NameId { get; set; }
        public string Name { get; set; }
    }
}