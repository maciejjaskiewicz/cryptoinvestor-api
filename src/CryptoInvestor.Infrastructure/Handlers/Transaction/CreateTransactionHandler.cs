using CryptoInvestor.Infrastructure.Commands;
using CryptoInvestor.Infrastructure.Commands.Transaction;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Handlers.Transaction
{
    public class CreateTransactionHandler : ICommandHandler<CreateTransaction>
    {
        private readonly ITransactionService _transactionService;

        public CreateTransactionHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task HandleAsync(CreateTransaction command)
        {
            await _transactionService.CreateAsync(command.Id, command.UserId, command.PortfolioNameId,
                command.CoinSymbol, command.PurchasePrice, command.PurchaseDate, command.Amount, command.Currency);
        }
    }
}