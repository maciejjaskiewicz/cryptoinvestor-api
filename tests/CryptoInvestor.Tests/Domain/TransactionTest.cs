using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Exceptions;
using Shouldly;
using System;
using Xunit;

namespace CryptoInvestor.Tests.Domain
{
    public class TransactionTest
    {
        private Transaction transaction;

        public TransactionTest()
        {
            var coin = new Coin("btc", "bitcoin");
            var date = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            transaction = new Transaction(Guid.NewGuid(), Guid.NewGuid(), "default", coin, 10M, date, 10, "USD");
        }

        [Fact]
        public void Given_valid_coin_to_SetCoin_should_set_it()
        {
            var coin = new Coin("eth", "etherium");
            transaction.SetCoin(coin);
            transaction.Coin.Symbol.ShouldBe("eth");
        }

        [Fact]
        public void Given_invalid_coin_SetCoin_should_throw_exception()
        {
            Coin coin = null;

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                transaction.SetCoin(coin);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidCoin);
        }

        [Fact]
        public void Given_valid_purchasePrice_to_SetPurchasePrice_should_set_it()
        {
            var price = 1000M;
            transaction.SetPurchasePrice(price);
            transaction.PurchasePrice.ShouldBe(price);
        }

        [Fact]
        public void Given_invalid_purchasePrice_SetPurchasePrice_should_throw_exception()
        {
            var price = -1000M;

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                transaction.SetPurchasePrice(price);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidPurchasePrice);
        }

        [Fact]
        public void Given_valid_purchaseDate_to_SetPurchaseDate_should_set_it()
        {
            var date = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            transaction.SetPurchaseDate(date);
            transaction.PurchaseDate.ShouldBe(date);
        }

        [Fact]
        public void Given_invalid_purchaseDate_SetPurchaseDate_should_throw_exception()
        {
            var date = 0;

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                transaction.SetPurchaseDate(date);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidPurchaseDate);
        }

        [Fact]
        public void Given_valid_amount_to_SetAmount_should_set_it()
        {
            var amount = 10;
            transaction.SetAmount(amount);
            transaction.Amount.ShouldBe(amount);
        }

        [Fact]
        public void Given_invalid_amount_SetAmount_should_throw_exception()
        {
            var amount = -10;

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                transaction.SetAmount(amount);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidAmount);
        }

        [Fact]
        public void Sold_should_set_IsSold_to_true_and_set_SoldDate_and_SoldPrice()
        {
            var price = 2000M;

            transaction.Sell(price);

            transaction.IsSold.ShouldBeTrue();
            transaction.SoldDate.ShouldNotBe(default(long));
            transaction.SoldPrice.ShouldBe(price);
        }

        [Fact]
        public void Given_valid_soldPrice_to_SetSoldPrice_should_set_it()
        {
            var price = 1000M;
            transaction.SetSoldPrice(price);
            transaction.SoldPrice.ShouldBe(price);
        }

        [Fact]
        public void Given_invalid_soldPrice_SetSoldPrice_should_throw_exception()
        {
            var price = -1000M;

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                transaction.SetSoldPrice(price);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidSoldPrice);
        }

        [Fact]
        public void Given_valid_soldDate_to_SetSoldDate_should_set_it()
        {
            var date = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            transaction.SetSoldDate(date);
            transaction.SoldDate.ShouldBe(date);
        }

        [Fact]
        public void Given_invalid_soldDate_SetSoldDate_should_throw_exception()
        {
            var date = 0;

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                transaction.SetSoldDate(date);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidSoldDate);
        }

        [Fact]
        public void SetProfit_should_set_profit()
        {
            var profit = 1000M;
            transaction.SetProfit(profit);
            transaction.Profit.ShouldBe(profit);
        }

        [Fact]
        public void Given_valid_curreny_to_SetCurreny_should_set_it()
        {
            var currency = "USD";
            transaction.SetCurrency(currency);
            transaction.Currency.ShouldBe(currency);
        }

        [Fact]
        public void Given_invalid_currency_SetCurrency_should_throw_exception()
        {
            var currency = "";

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                transaction.SetCurrency(currency);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidCurrency);
        }
    }
}