using CryptoInvestor.Core.Exceptions;
using CryptoInvestor.Core.Extensions.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoInvestor.Core.Domain
{
    public class Coin
    {
        public Guid Id { get; protected set; }
        public string Symbol { get; protected set; }
        public string Name { get; protected set; }
        public decimal MarketCap { get; protected set; }
        public decimal Change24h { get; protected set; }
        public decimal Volume24h { get; protected set; }
        public string IconUrl { get; protected set; }

        public IEnumerable<CoinPrice> Prices
        {
            get { return _prices; }
            set { _prices = new HashSet<CoinPrice>(value); }
        }

        private ISet<CoinPrice> _prices = new HashSet<CoinPrice>();

        protected Coin()
        {
        }

        public Coin(string symbol, string name)
        {
            Id = new Guid();
            SetSymbol(symbol);
            SetName(name);
        }

        public void SetSymbol(string symbol)
        {
            if (symbol.Empty())
            {
                throw new DomainException(ErrorCodes.InvalidCoinSymbol,
                    "Coin symbol can not be empty.");
            }

            Symbol = symbol.ToLowerInvariant();
        }

        public void SetName(string name)
        {
            if (name.Empty())
            {
                throw new DomainException(ErrorCodes.InvalidCoinName,
                    "Coin name can not be empty.");
            }

            Name = name;
        }

        public void AddPrice(CoinPrice price)
        {
            if (!_prices.Any(x => x.Currency == price.Currency))
            {
                _prices.Add(price);
            }
        }

        public void UpdatePrice(CoinPrice price)
        {
            var priceWithTheSameCurrency = _prices.SingleOrDefault(x => x.Currency == price.Currency);

            if (priceWithTheSameCurrency != null)
            {
                _prices.Remove(priceWithTheSameCurrency);
                _prices.Add(price);
            }
        }

        public void ClearPrices()
        {
            _prices.Clear();
        }

        public void SetMarketCap(decimal marketCap)
        {
            MarketCap = marketCap;
        }

        public void SetChange24h(decimal change24h)
        {
            Change24h = change24h;
        }

        public void SetVolume24h(decimal volume24h)
        {
            Volume24h = volume24h;
        }

        public void SetIconUrl(string iconUrl)
        {
            if (iconUrl.Empty())
            {
                throw new DomainException(ErrorCodes.InvalidIconUrl,
                    "Icon URL can not be empty.");
            }

            IconUrl = iconUrl;
        }
    }
}