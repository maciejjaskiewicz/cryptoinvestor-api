using CryptoInvestor.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CryptoInvestor.Core.Domain
{
    public class Favourites
    {
        public Guid Id { get; protected set; }

        public IEnumerable<Coin> Coins
        {
            get { return _coins; }
            set { _coins = new HashSet<Coin>(value); }
        }

        private ISet<Coin> _coins = new HashSet<Coin>();

        protected Favourites()
        {
        }

        public Favourites(Guid userId)
        {
            Id = userId;
        }

        public void UpdateCoin(Coin coin)
        {
            var localCoin = _coins.SingleOrDefault(x => x.Symbol == coin.Symbol);

            if (localCoin != null)
            {
                _coins.Remove(localCoin);
                _coins.Add(coin);
            }
        }

        public void AddCoin(Coin coin)
        {
            if (coin == null)
            {
                throw new DomainException(ErrorCodes.InvalidCoin,
                    "Coin can not be null.");
            }

            var localCoin = _coins.SingleOrDefault(x => x.Symbol == coin.Symbol);
            if (localCoin != null)
            {
                throw new DomainException(ErrorCodes.CoinAlreadyExists,
                    $"Coin with symbol: {coin.Symbol} already exists for favourites collection with id: {Id}");
            }

            _coins.Add(coin);
        }

        public void RemoveCoin(Coin coin)
        {
            var localCoin = _coins.SingleOrDefault(x => x.Symbol == coin.Symbol);
            if (localCoin == null)
            {
                throw new DomainException(ErrorCodes.CoinNotFound,
                    $"Coin with symbol: {coin.Symbol} was not found for favourites collection with id: {Id}.");
            }

            _coins.Remove(localCoin);
        }
    }
}