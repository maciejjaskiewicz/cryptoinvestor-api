using CryptoInvestor.Infrastructure.Commands;
using CryptoInvestor.Infrastructure.Commands.Portfolio;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Handlers.Portfolio
{
    public class CreatePortfolioHandler : ICommandHandler<CreatePortfolio>
    {
        private readonly IPortfolioService _portfolioService;

        public CreatePortfolioHandler(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public async Task HandleAsync(CreatePortfolio command)
        {
            await _portfolioService.CreateAsync(command.UserId, command.Name);
        }
    }
}