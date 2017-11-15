using CryptoInvestor.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Services.Interfaces
{
    public interface ITransactionService : IService
    {
        Task<TransactionDto> GetAsync(Guid id);
        Task<IEnumerable<TransactionDto>> BrowseAsync(Guid userId);
        Task<IEnumerable<TransactionDto>> BrowseAsync(Guid userId, string portfolioNameId);
        Task CreateAsync(Guid id, Guid userId, string portfolioNameId, string coinSymbol, 
            decimal purchasePrice, long purchaseDate, decimal amount, string currency);
        Task UpdateAsync(Guid id, string coinSymbol,
            decimal purchasePrice, long purchaseDate, decimal amount, string currency);
        Task SellAsync(Guid id, decimal soldPrice);
        Task DeleteAsync(Guid id);
    }
}
