using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Exceptions;
using Shouldly;
using System;
using Xunit;

namespace CryptoInvestor.Tests.Domain
{
    public class PortfolioTest
    {
        private Portfolio portfolio;

        public PortfolioTest()
        {
            portfolio = new Portfolio(Guid.NewGuid(), "test");
        }

        [Fact]
        public void Given_valid_name_SetName_should_set_it()
        {
            var name = "test1";
            portfolio.SetName(name);
            portfolio.Name.ShouldBe(name);
        }

        [Fact]
        public void Given_invalid_name_SetName_should_throw_exception()
        {
            var name = "";

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                portfolio.SetName(name);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidPortfolioName);
        }

        [Fact]
        public void SetName_should_set_portfolioName_id()
        {
            var name = "Portfolio name ID test";
            portfolio.SetName(name);
            portfolio.NameId.ShouldBe("portfolio-name-id-test");
        }
    }
}