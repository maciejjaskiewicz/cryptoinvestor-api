using CryptoInvestor.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoInvestor.Core.Repositories
{
    public interface IUserRepository : IRepository
    {
        Task AddAsync(User user);
        Task<User> GetAsync(Guid id);
        Task<User> GetAsync(string email);
        Task<IEnumerable<User>> BrowseAsync();
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }
}
