using CryptoInvestor.Infrastructure.Commands;
using CryptoInvestor.Infrastructure.Commands.Portfolio;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Handlers.Portfolio
{
    public class UpdatePortfolioHandler : ICommandHandler<UpdatePortfolio>
    {
        private readonly IPortfolioService _portfolioService;

        public UpdatePortfolioHandler(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public async Task HandleAsync(UpdatePortfolio command)
        {
            var portfolio = await _portfolioService.GetAsync(command.UserId, command.NameId);
            await _portfolioService.UpdateAsync(portfolio.Id, command.Name);
        }
    }
}