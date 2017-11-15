using CryptoInvestor.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoInvestor.Core.Repositories
{
    public interface IPortfolioRepository : IRepository
    {
        Task AddAsync(Portfolio portfolio);
        Task<Portfolio> GetAsync(Guid id);
        Task<IEnumerable<Portfolio>> BrowseAsync(Guid userId);
        Task UpdateAsync(Portfolio portfolio);
        Task DeleteAsync(Guid id);
    }
}
