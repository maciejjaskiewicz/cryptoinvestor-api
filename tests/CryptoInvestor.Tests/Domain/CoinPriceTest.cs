using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Exceptions;
using Shouldly;
using Xunit;

namespace CryptoInvestor.Tests.Domain
{
    public class CoinPriceTest
    {
        [Fact]
        public void Given_valid_parameters_CoinPrice_should_be_created()
        {
            CoinPrice coinPrice = CoinPrice.Create("USD", 1000M);

            coinPrice.Currency.ShouldBe("USD");
            coinPrice.Price.ShouldBe(1000M);
        }

        [Fact]
        public void Given_invalid_parameters_Create_should_throw_proper_exception()
        {
            DomainException ex = Should.Throw<DomainException>(() =>
            {
                CoinPrice coinPrice = CoinPrice.Create("", 1000M);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidCurrency);
        }
    }
}