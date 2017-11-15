using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Exceptions;
using Shouldly;
using System.Linq;
using Xunit;

namespace CryptoInvestor.Tests.Domain
{
    public class CoinTest
    {
        private Coin coin;

        public CoinTest()
        {
            coin = new Coin("btc", "bitcoin");
        }

        [Fact]
        public void Given_valid_symbol_should_be_changed()
        {
            var newSymbol = "eth";
            coin.SetSymbol(newSymbol);
            coin.Symbol.ShouldBe(newSymbol);
        }

        [Fact]
        public void Given_invalid_symbol_SetSymbol_should_throw_proper_exception()
        {
            var newSymbol = "";

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                coin.SetSymbol(newSymbol);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidCoinSymbol);
        }

        [Fact]
        public void Given_valid_name_should_be_changed()
        {
            var newName = "etherium";
            coin.SetName(newName);
            coin.Name.ShouldBe(newName);
        }

        [Fact]
        public void Given_invalid_name_SetName_should_throw_proper_exception()
        {
            var newName = "";

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                coin.SetName(newName);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidCoinName);
        }

        [Fact]
        public void Given_valid_iconUrl_should_be_set()
        {
            var iconUrl = "url";
            coin.SetIconUrl(iconUrl);
            coin.IconUrl.ShouldBe(iconUrl);
        }

        [Fact]
        public void Given_invalid_iconUrl_should_throw_proper_exception()
        {
            var iconUrl = "";

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                coin.SetIconUrl(iconUrl);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidIconUrl);
        }

        [Fact]
        public void Given_valid_price_AddPrice_should_add_new_coinPrice()
        {
            var newPrice = CoinPrice.Create("USD", 1000M);
            coin.AddPrice(newPrice);
            coin.Prices.SingleOrDefault(x => x.Currency == "USD").ShouldNotBeNull();
        }

        [Fact]
        public void Given_already_exists_currencyPrice_should_not_be_added()
        {
            var newPrice = CoinPrice.Create("USD", 1000M);
            coin.AddPrice(newPrice);

            coin.AddPrice(newPrice);

            coin.Prices.Count(x => x.Currency == "USD").ShouldBe(1);
        }

        [Fact]
        public void Given_valid_price_UpdatePrice_should_update_the_price()
        {
            var price = CoinPrice.Create("USD", 1000M);
            coin.AddPrice(price);

            var newPrice = CoinPrice.Create("USD", 2000M);
            coin.UpdatePrice(newPrice);

            price = coin.Prices.SingleOrDefault(x => x.Currency == "USD");
            price.Price.ShouldBe(2000M);
        }

        [Fact]
        public void ClearPrices_should_clear_all_prices()
        {
            var newPrice = CoinPrice.Create("USD", 1000M);
            coin.AddPrice(newPrice);

            coin.ClearPrices();

            coin.Prices.Any().ShouldBeFalse();
        }

        [Fact]
        public void SetMarketCap_should_set_proper_value()
        {
            coin.SetMarketCap(1000M);
            coin.MarketCap.ShouldBe(1000M);
        }

        [Fact]
        public void SetVolume24h_should_set_proper_value()
        {
            coin.SetVolume24h(1000M);
            coin.Volume24h.ShouldBe(1000M);
        }

        [Fact]
        public void SetChange24h_should_set_proper_value()
        {
            coin.SetChange24h(10M);
            coin.Change24h.ShouldBe(10M);
        }
    }
}