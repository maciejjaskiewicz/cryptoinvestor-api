using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Repositories
{
    public class FavouritesRepository : IFavouritesRepository, IMongoRepository
    {
        private readonly IMongoDatabase _database;

        public FavouritesRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(Favourites favourites) =>
            await Favourites.InsertOneAsync(favourites);

        public async Task<Favourites> GetAsync(Guid userId) =>
            await Favourites.AsQueryable().SingleOrDefaultAsync(x => x.Id == userId);

        public async Task UpdateAsync(Favourites favourites) =>
            await Favourites.ReplaceOneAsync(x => x.Id == favourites.Id, favourites);

        public async Task DeleteAsync(Guid userId) =>
            await Favourites.DeleteOneAsync(x => x.Id == userId);

        private IMongoCollection<Favourites> Favourites => _database.GetCollection<Favourites>("Favourites");
    }
}