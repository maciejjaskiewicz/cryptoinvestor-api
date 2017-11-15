using CryptoInvestor.Core.Exceptions;
using CryptoInvestor.Core.Extensions.Validations;

namespace CryptoInvestor.Core.Domain
{
    public class CoinPrice
    {
        public string Currency { get; protected set; }
        public decimal Price { get; protected set; }

        protected CoinPrice()
        {
        }

        protected CoinPrice(string currencySymbol, decimal price)
        {
            ValidateCurrencySymbolOrFail(currencySymbol);
            currencySymbol.ToUpperInvariant();

            Currency = currencySymbol;
            Price = price;
        }

        private void ValidateCurrencySymbolOrFail(string currencySymbol)
        {
            if (currencySymbol.Empty())
            {
                throw new DomainException(ErrorCodes.InvalidCurrency,
                    "Currency symbol can not be empty.");
            }
        }

        public static CoinPrice Create(string currencySymbol, decimal price)
            => new CoinPrice(currencySymbol, price);
    }
}