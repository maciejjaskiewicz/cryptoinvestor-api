using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository, IMongoRepository
    {
        private readonly IMongoDatabase _database;

        public TransactionRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(Transaction transaction) =>
            await Transactions.InsertOneAsync(transaction);

        public async Task<Transaction> GetAsync(Guid id) =>
            await Transactions.AsQueryable().SingleOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<Transaction>> BrowseAsync(Guid userId) =>
            await Transactions.AsQueryable().Where(x => x.UserId == userId).ToListAsync();

        public async Task UpdateAsync(Transaction transaction) =>
            await Transactions.ReplaceOneAsync(x => x.Id == transaction.Id, transaction);

        public async Task DeleteAsync(Guid id) =>
            await Transactions.DeleteOneAsync(x => x.Id == id);

        private IMongoCollection<Transaction> Transactions => _database.GetCollection<Transaction>("Transactions");
    }
}