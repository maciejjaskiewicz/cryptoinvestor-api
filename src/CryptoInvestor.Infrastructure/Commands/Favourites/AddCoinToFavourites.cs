using CryptoInvestor.Infrastructure.Commands.Auth;

namespace CryptoInvestor.Infrastructure.Commands.Favourites
{
    public class AddCoinToFavourites : AuthenticatedCommandBase
    {
        public string CoinSymbol { get; set; }
    }
}