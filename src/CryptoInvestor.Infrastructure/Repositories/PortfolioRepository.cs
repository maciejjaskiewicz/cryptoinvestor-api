using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Repositories
{
    public class PortfolioRepository : IPortfolioRepository, IMongoRepository
    {
        private readonly IMongoDatabase _database;

        public PortfolioRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(Portfolio portfolio) =>
            await Portfolios.InsertOneAsync(portfolio);

        public async Task<Portfolio> GetAsync(Guid id) =>
            await Portfolios.AsQueryable().SingleOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Portfolio>> BrowseAsync(Guid userId) =>
            await Portfolios.AsQueryable().Where(x => x.UserId == userId).ToListAsync();

        public async Task UpdateAsync(Portfolio portfolio) =>
            await Portfolios.ReplaceOneAsync(x => x.Id == portfolio.Id, portfolio);

        public async Task DeleteAsync(Guid id) =>
            await Portfolios.DeleteOneAsync(x => x.Id == id);

        private IMongoCollection<Portfolio> Portfolios => _database.GetCollection<Portfolio>("Portfolios");
    }
}