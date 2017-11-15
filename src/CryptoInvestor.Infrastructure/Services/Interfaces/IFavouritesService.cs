using CryptoInvestor.Infrastructure.DTO;
using System;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Services.Interfaces
{
    public interface IFavouritesService : IService
    {
        Task CreateAsync(Guid userId);
        Task<FavouritesDto> GetAsync(Guid userId);
        Task<CoinDto> GetCoinAsync(Guid userId, string coinSymbol);
        Task AddCoinAsync(Guid userId, string coinSymbol);
        Task DeleteCoinAsync(Guid userId, string coinSymbol);
    }
}
