using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private static ISet<User> _users = new HashSet<User>();

        public async Task AddAsync(User user)
        {
            _users.Add(user);
            await Task.CompletedTask;
        }

        public async Task<User> GetAsync(Guid id) =>
            await Task.FromResult(_users.SingleOrDefault(x => x.Id == id));

        public async Task<User> GetAsync(string email) =>
            await Task.FromResult(_users.SingleOrDefault(x => x.Email == email));

        public async Task<IEnumerable<User>> BrowseAsync() =>
            await Task.FromResult(_users);

        public async Task UpdateAsync(User user)
        {
            var userToUpdate = await Task.FromResult(_users.SingleOrDefault(x => x.Id == user.Id));
            if (userToUpdate != null)
            {
                userToUpdate.Update(user.FirstName, user.LastName, user.Gender);
            }
            await Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}