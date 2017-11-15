using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository, IMongoRepository
    {
        private readonly IMongoDatabase _database;

        public UserRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task AddAsync(User user) =>
            await Users.InsertOneAsync(user);

        public async Task<User> GetAsync(Guid id) =>
            await Users.AsQueryable().SingleOrDefaultAsync(x => x.Id == id);

        public async Task<User> GetAsync(string email) =>
            await Users.AsQueryable().SingleOrDefaultAsync(x => x.Email == email);

        public async Task<IEnumerable<User>> BrowseAsync() =>
            await Users.AsQueryable().ToListAsync();

        public async Task UpdateAsync(User user) =>
            await Users.ReplaceOneAsync(x => x.Id == user.Id, user);

        public async Task DeleteAsync(Guid id) =>
            await Users.DeleteOneAsync(x => x.Id == id);

        private IMongoCollection<User> Users => _database.GetCollection<User>("Users");
    }
}