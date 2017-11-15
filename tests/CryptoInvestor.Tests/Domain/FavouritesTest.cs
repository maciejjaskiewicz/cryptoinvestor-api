using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Exceptions;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace CryptoInvestor.Tests.Domain
{
    public class FavouritesTest
    {
        private Favourites favourites;

        public FavouritesTest()
        {
            favourites = new Favourites(Guid.NewGuid());
        }

        [Fact]
        public void Given_valid_coin_to_AddCoin_should_be_added()
        {
            var coin = new Coin("btc", "bitcoin");
            favourites.AddCoin(coin);
            favourites.Coins.SingleOrDefault(x => x.Symbol == "btc").ShouldNotBeNull();
        }

        [Fact]
        public void Given_invalid_coin_AddCoin_should_throw_exception()
        {
            Coin coin = null;

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                favourites.AddCoin(coin);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidCoin);
        }

        [Fact]
        public void Given_exists_coin_AddCoin_should_throw_exception()
        {
            var coin = new Coin("btc", "bitcoin");
            favourites.AddCoin(coin);

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                favourites.AddCoin(coin);
            });

            ex.Code.ShouldBe(ErrorCodes.CoinAlreadyExists);
        }

        [Fact]
        public void Given_exists_coin_RemoveCoin_should_remove_it()
        {
            var coin = new Coin("btc", "bitcoin");
            favourites.AddCoin(coin);

            favourites.RemoveCoin(coin);

            favourites.Coins.Any().ShouldBeFalse();
        }

        [Fact]
        public void Given_not_exists_coin_Remove_Coin_should_throw_exception()
        {
            var coin = new Coin("btc", "bitcoin");

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                favourites.RemoveCoin(coin);
            });

            ex.Code.ShouldBe(ErrorCodes.CoinNotFound);
        }
    }
}