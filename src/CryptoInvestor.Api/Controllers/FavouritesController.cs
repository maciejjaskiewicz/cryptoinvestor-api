using CryptoInvestor.Infrastructure.Commands;
using CryptoInvestor.Infrastructure.Commands.Favourites;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CryptoInvestor.Api.Controllers
{
    [Authorize]
    public class FavouritesController : ApiControllerBase
    {
        private readonly IFavouritesService _favouritesService;

        public FavouritesController(ICommandDispatcher commandDispatcher,
            IFavouritesService favouritesService) : base(commandDispatcher)
        {
            _favouritesService = favouritesService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var favourites = await _favouritesService.GetAsync(UserId);
            if (favourites == null)
            {
                return NotFound();
            }

            return Json(favourites);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]AddCoinToFavourites command)
        {
            await DispatchAsync(command);

            return Ok();
        }

        [HttpGet("{coinSymbol}")]
        public async Task<IActionResult> Get(string coinSymbol)
        {
            var coin = await _favouritesService.GetCoinAsync(UserId, coinSymbol);
            if (coin == null)
            {
                return NotFound();
            }

            return Json(coin);
        }

        [HttpDelete("{coinSymbol}")]
        public async Task<IActionResult> Delete(string coinSymbol)
        {
            await _favouritesService.DeleteCoinAsync(UserId, coinSymbol);

            return Ok();
        }
    }
}