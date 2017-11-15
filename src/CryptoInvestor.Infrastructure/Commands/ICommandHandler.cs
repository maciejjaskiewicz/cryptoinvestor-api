using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Commands
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task HandleAsync(T command);
    }
}