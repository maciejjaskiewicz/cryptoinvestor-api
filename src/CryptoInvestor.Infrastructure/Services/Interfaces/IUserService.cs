using CryptoInvestor.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Services.Interfaces
{
    public interface IUserService : IService
    {
        Task<UserDetailsDto> GetAsync(string email);
        Task<UserDetailsDto> GetAsync(Guid id);
        Task<IEnumerable<UserDto>> BrowseAsync();
        Task LoginAsync(string email, string password);
        Task RegisterAsync(string email, string username, string password);
        Task UpdateAsync(string email, string firstName, string lastName, string gender);
        Task DeleteAsync(Guid id);
    }
}
