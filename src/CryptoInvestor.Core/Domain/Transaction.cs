using CryptoInvestor.Core.Exceptions;
using CryptoInvestor.Core.Extensions.Validations;
using System;

namespace CryptoInvestor.Core.Domain
{
    public class Transaction
    {
        public Guid Id { get; protected set; }
        public Guid UserId { get; protected set; }
        public string PortfolioNameId { get; protected set; }
        public Coin Coin { get; protected set; }
        public decimal PurchasePrice { get; protected set; }
        public long PurchaseDate { get; protected set; }
        public decimal Amount { get; protected set; }
        public decimal Profit { get; protected set; }
        public string Currency { get; protected set; }
        public bool IsSold { get; protected set; }
        public decimal SoldPrice { get; protected set; }
        public long SoldDate { get; protected set; }

        protected Transaction()
        {
        }

        public Transaction(Guid id, Guid userId, string portfolioNameId, Coin coin, decimal purchasePrice,
            long purchaseDate, decimal amount, string currency)
        {
            Id = id;
            UserId = userId;
            PortfolioNameId = portfolioNameId;
            IsSold = false;

            SetCoin(coin);
            SetPurchaseDate(purchaseDate);
            SetPurchasePrice(purchasePrice);
            SetAmount(amount);
            SetCurrency(currency);
        }

        public void Sell(decimal soldPrice)
        {
            IsSold = true;
            SoldDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            SetSoldPrice(soldPrice);
        }

        public void SetCoin(Coin coin)
        {
            Coin = coin ?? throw new DomainException(ErrorCodes.InvalidCoin,
                    "Coin can not be null.");
        }

        public void SetPurchasePrice(decimal purchasePrice)
        {
            if (purchasePrice < 0M)
            {
                throw new DomainException(ErrorCodes.InvalidPurchasePrice,
                    "Purchase price must be greater than zero.");
            }

            PurchasePrice = purchasePrice;
        }

        public void SetPurchaseDate(long purchaseDate)
        {
            if (purchaseDate <= 0)
            {
                throw new DomainException(ErrorCodes.InvalidPurchaseDate,
                    "Invalid purchase date.");
            }

            PurchaseDate = purchaseDate;
        }

        public void SetAmount(decimal amount)
        {
            if (amount <= 0M)
            {
                throw new DomainException(ErrorCodes.InvalidAmount,
                    "Amount of coins must be greater than zero..");
            }

            Amount = amount;
        }

        public void SetSoldPrice(decimal soldPrice)
        {
            if (soldPrice < 0M)
            {
                throw new DomainException(ErrorCodes.InvalidSoldPrice,
                    "Sold price must be greater than zero.");
            }

            SoldPrice = soldPrice;
        }

        public void SetSoldDate(long soldDate)
        {
            if (soldDate <= 0)
            {
                throw new DomainException(ErrorCodes.InvalidSoldDate,
                    "Invalid sold date.");
            }

            SoldDate = soldDate;
        }

        public void SetProfit(decimal profit)
        {
            Profit = profit;
        }

        public void SetCurrency(string currency)
        {
            if (currency.Empty())
            {
                throw new DomainException(ErrorCodes.InvalidCurrency,
                    "Currency symbol can not be empty.");
            }

            Currency = currency;
        }
    }
}