using CryptoInvestor.Infrastructure.Commands;
using CryptoInvestor.Infrastructure.Commands.Transaction;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Handlers.Transaction
{
    public class UpdateTransactionHandler : ICommandHandler<UpdateTransaction>
    {
        private readonly ITransactionService _transactionService;

        public UpdateTransactionHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task HandleAsync(UpdateTransaction command)
        {
            await _transactionService.UpdateAsync(command.Id, command.CoinSymbol, command.PurchasePrice,
                command.PurchaseDate, command.Amount, command.Currency);

            if (command.Sold)
            {
                await _transactionService.SellAsync(command.Id, command.SoldPrice);
            }
        }
    }
}