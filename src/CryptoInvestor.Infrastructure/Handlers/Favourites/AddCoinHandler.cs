using CryptoInvestor.Infrastructure.Commands;
using CryptoInvestor.Infrastructure.Commands.Favourites;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Handlers.Favourites
{
    public class AddCoinHandler : ICommandHandler<AddCoinToFavourites>
    {
        private readonly IFavouritesService _favouritesService;

        public AddCoinHandler(IFavouritesService favouritesService)
        {
            _favouritesService = favouritesService;
        }

        public async Task HandleAsync(AddCoinToFavourites command)
        {
            await _favouritesService.AddCoinAsync(command.UserId, command.CoinSymbol);
        }
    }
}