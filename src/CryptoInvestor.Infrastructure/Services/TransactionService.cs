using AutoMapper;
using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Repositories;
using CryptoInvestor.Infrastructure.DTO;
using CryptoInvestor.Infrastructure.Exceptions;
using CryptoInvestor.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoInvestor.Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly ICoinRepository _coinRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository,
            IPortfolioRepository portfolioRepository, IUserRepository userRepository,
            ICoinRepository coinRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _portfolioRepository = portfolioRepository;
            _userRepository = userRepository;
            _coinRepository = coinRepository;
            _mapper = mapper;
        }

        public async Task<TransactionDto> GetAsync(Guid id)
        {
            var transaction = await _transactionRepository.GetAsync(id);

            if (transaction != null)
            {
                transaction = await CalculateProfitAsync(transaction);
            }

            return _mapper.Map<TransactionDto>(transaction);
        }

        public async Task<IEnumerable<TransactionDto>> BrowseAsync(Guid userId)
        {
            var transactions = await _transactionRepository.BrowseAsync(userId);
            transactions = await CalculateProfitAsync(transactions);

            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public async Task<IEnumerable<TransactionDto>> BrowseAsync(Guid userId, string portfolioNameId)
        {
            var transactions = await _transactionRepository.BrowseAsync(userId);
            var portfolioTransactions = transactions.Where(x => x.PortfolioNameId == portfolioNameId);
            portfolioTransactions = await CalculateProfitAsync(portfolioTransactions);

            return _mapper.Map<IEnumerable<TransactionDto>>(portfolioTransactions);
        }

        public async Task CreateAsync(Guid id, Guid userId, string portfolioNameId, string coinSymbol,
            decimal purchasePrice, long purchaseDate, decimal amount, string currency)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
            {
                throw new ServiceException(ErrorCodes.UserNotFound,
                    $"User with id: {userId} does not exists.");
            }

            var portfolios = await _portfolioRepository.BrowseAsync(userId);
            var portfolio = portfolios.SingleOrDefault(x => x.NameId == portfolioNameId);
            if (portfolio == null)
            {
                throw new ServiceException(ErrorCodes.PortfolioNotFound,
                    $"Portfolio with id: {portfolioNameId} does not exists.");
            }

            var coin = await _coinRepository.GetAsync(coinSymbol.ToLowerInvariant());
            if (coin == null)
            {
                throw new ServiceException(ErrorCodes.InvalidCoin,
                    $"Coin with symbol: {coinSymbol} was not found.");
            }

            var transaction = new Transaction(id, userId, portfolioNameId, coin,
                purchasePrice, purchaseDate, amount, currency);

            await _transactionRepository.AddAsync(transaction);
        }

        public async Task UpdateAsync(Guid id, string coinSymbol, decimal purchasePrice,
            long purchaseDate, decimal amount, string currency)
        {
            var transaction = await _transactionRepository.GetAsync(id);
            if (transaction == null)
            {
                throw new ServiceException(ErrorCodes.TransactionNotFound,
                    $"Transaction with id: {id} was not found.");
            }

            var coin = await _coinRepository.GetAsync(coinSymbol.ToLowerInvariant());
            if (coin == null)
            {
                throw new ServiceException(ErrorCodes.InvalidCoin,
                    $"Coin with symbol: {coinSymbol} was not found.");
            }

            transaction.SetCoin(coin);
            transaction.SetPurchasePrice(purchasePrice);
            transaction.SetPurchaseDate(purchaseDate);
            transaction.SetAmount(amount);
            transaction.SetCurrency(currency);

            await _transactionRepository.UpdateAsync(transaction);
        }

        public async Task SellAsync(Guid id, decimal soldPrice)
        {
            var transaction = await _transactionRepository.GetAsync(id);
            if (transaction == null)
            {
                throw new ServiceException(ErrorCodes.TransactionNotFound,
                    $"Transaction with id: {id} was not found.");
            }

            transaction.Sell(soldPrice);

            await _transactionRepository.UpdateAsync(transaction);
        }

        public async Task DeleteAsync(Guid id)
        {
            var transaction = await _transactionRepository.GetAsync(id);
            if (transaction == null)
            {
                throw new ServiceException(ErrorCodes.TransactionNotFound,
                    $"Transaction with id: {id} was not found.");
            }

            await _transactionRepository.DeleteAsync(id);
        }

        private async Task<IEnumerable<Transaction>> CalculateProfitAsync(IEnumerable<Transaction> transactions)
        {
            IList<Transaction> tempTransactions = new List<Transaction>();
            foreach (var transaction in transactions)
            {
                var updatedTransaction = await CalculateProfitAsync(transaction);
                tempTransactions.Add(updatedTransaction);
            }

            return tempTransactions;
        }

        private async Task<Transaction> CalculateProfitAsync(Transaction transaction)
        {
            var coin = await _coinRepository.GetAsync(transaction.Coin.Symbol);
            transaction.SetCoin(coin);

            var currentPrice = coin.Prices.SingleOrDefault(x => x.Currency == "USD").Price;
            var profit = (transaction.Amount * currentPrice) - (transaction.Amount * transaction.PurchasePrice);
            transaction.SetProfit(profit);

            return transaction;
        }
    }
}