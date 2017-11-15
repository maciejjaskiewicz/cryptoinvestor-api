using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Services.Interfaces
{
    public interface ICoinsProvider
    {
        Task Provide();
    }
}