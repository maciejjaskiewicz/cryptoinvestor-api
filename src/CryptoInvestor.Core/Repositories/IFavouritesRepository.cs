using CryptoInvestor.Core.Domain;
using System;
using System.Threading.Tasks;

namespace CryptoInvestor.Core.Repositories
{
    public interface IFavouritesRepository : IRepository
    {
        Task AddAsync(Favourites favourites);
        Task<Favourites> GetAsync(Guid userId);
        Task UpdateAsync(Favourites favourites);
        Task DeleteAsync(Guid userId);
    }
}
