using CryptoInvestor.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoInvestor.Core.Repositories
{
    public interface ITransactionRepository : IRepository
    {
        Task AddAsync(Transaction transaction);
        Task<Transaction> GetAsync(Guid id);
        Task<IEnumerable<Transaction>> BrowseAsync(Guid userId);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(Guid id);
    }
}
