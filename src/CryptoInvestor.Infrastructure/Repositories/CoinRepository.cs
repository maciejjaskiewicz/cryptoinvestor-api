using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Repositories
{
    public class CoinRepository : ICoinRepository, IMongoRepository
    {
        private readonly IMongoDatabase _database;

        public CoinRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(Coin coin) =>
            await Coins.InsertOneAsync(coin);

        public async Task<Coin> GetAsync(Guid id) =>
            await Coins.AsQueryable().SingleOrDefaultAsync(x => x.Id == id);

        public async Task<Coin> GetAsync(string symbol) =>
            await Coins.AsQueryable().SingleOrDefaultAsync(x => x.Symbol == symbol);

        public async Task<IEnumerable<Coin>> BrowseAsync() =>
            await Coins.AsQueryable().ToListAsync();

        public async Task UpdateAsync(Coin coin) =>
            await Coins.ReplaceOneAsync(x => x.Id == coin.Id, coin);

        public async Task DeleteAsync(Guid id) =>
            await Coins.DeleteOneAsync(x => x.Id == id);

        private IMongoCollection<Coin> Coins => _database.GetCollection<Coin>("Coins");
    }
}