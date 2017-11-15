using CryptoInvestor.Infrastructure.Commands;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CryptoInvestor.Api.Controllers
{
    public class CoinsController : ApiControllerBase
    {
        private readonly ICoinService _coinService;

        public CoinsController(ICommandDispatcher commandDispatcher,
            ICoinService coinService) : base(commandDispatcher)
        {
            _coinService = coinService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery(Name = "short")] bool? shortCollection)
        {
            if (shortCollection == true)
            {
                var coins = await _coinService.BrowseShortAsync();
                return Json(coins);
            }
            else
            {
                var coins = await _coinService.BrowseAsync();
                return Json(coins);
            }
        }

        [HttpGet("{symbol}")]
        public async Task<IActionResult> Get(string symbol)
        {
            var coin = await _coinService.GetAsync(symbol);

            if (coin == null)
            {
                return NotFound();
            }

            return Json(coin);
        }
    }
}