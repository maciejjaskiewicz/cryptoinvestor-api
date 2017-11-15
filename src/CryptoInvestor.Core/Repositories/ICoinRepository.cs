using CryptoInvestor.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoInvestor.Core.Repositories
{
    public interface ICoinRepository : IRepository
    {
        Task AddAsync(Coin coin);
        Task<Coin> GetAsync(Guid id);
        Task<Coin> GetAsync(string symbol);
        Task<IEnumerable<Coin>> BrowseAsync();
        Task UpdateAsync(Coin coin);
        Task DeleteAsync(Guid id);
    }
}
