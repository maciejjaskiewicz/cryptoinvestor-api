using CryptoInvestor.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Services.Interfaces
{
    public interface IPortfolioService : IService
    {
        Task<PortfolioDto> GetAsync(Guid id);
        Task<PortfolioDto> GetAsync(Guid userId, string nameId);
        Task<IEnumerable<PortfolioDto>> BrowseAsync(Guid userId);
        Task CreateAsync(Guid userId, string name);
        Task UpdateAsync(Guid id, string name);
        Task DeleteAsync(Guid id);
    }
}
