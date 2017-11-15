using CryptoInvestor.Infrastructure.Commands;
using CryptoInvestor.Infrastructure.Commands.Portfolio;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CryptoInvestor.Api.Controllers
{
    [Authorize]
    public class PortfolioController : ApiControllerBase
    {
        private readonly IPortfolioService _portfolioService;
        private readonly ITransactionService _transactionService;

        public PortfolioController(IPortfolioService portfolioService,
            ITransactionService transactionService, ICommandDispatcher commandDispatcher)
            : base(commandDispatcher)
        {
            _portfolioService = portfolioService;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var portfolios = await _portfolioService.BrowseAsync(UserId);
            return Json(portfolios);
        }

        [HttpGet("{nameId}")]
        public async Task<IActionResult> Get(string nameId)
        {
            var portfolio = await _portfolioService.GetAsync(UserId, nameId);

            if (portfolio == null)
            {
                return NotFound();
            }

            return Json(portfolio);
        }

        [HttpGet("{nameId}/transactions")]
        public async Task<IActionResult> GetTransactions(string nameId)
        {
            var transactions = await _transactionService.BrowseAsync(UserId, nameId);

            if (transactions == null)
            {
                return NotFound();
            }

            return Json(transactions);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreatePortfolio command)
        {
            await DispatchAsync(command);
            var nameId = command.Name.ToLowerInvariant().Trim().Replace(' ', '-');
            return Created($"portfolio/{nameId}", null);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]UpdatePortfolio command)
        {
            await DispatchAsync(command);
            return Ok();
        }

        [HttpDelete("{nameId}")]
        public async Task<IActionResult> Delete(string nameId)
        {
            var portfolio = await _portfolioService.GetAsync(UserId, nameId);

            if (portfolio == null)
            {
                return NotFound();
            }

            await _portfolioService.DeleteAsync(portfolio.Id);
            return Ok();
        }
    }
}