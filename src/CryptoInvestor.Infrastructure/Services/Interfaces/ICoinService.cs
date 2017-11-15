using CryptoInvestor.Infrastructure.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Services.Interfaces
{
    public interface ICoinService : IService
    {
        Task<CoinDto> GetAsync(string symbol);
        Task<IEnumerable<CoinDto>> BrowseAsync();
        Task<IEnumerable<CoinShortDto>> BrowseShortAsync();
    }
}
