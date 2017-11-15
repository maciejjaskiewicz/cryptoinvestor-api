using CryptoInvestor.Infrastructure.Commands.Auth;
using System;

namespace CryptoInvestor.Infrastructure.Commands.Transaction
{
    public class CreateTransaction : AuthenticatedCommandBase
    {
        public Guid Id { get; set; }
        public string PortfolioNameId { get; set; }
        public string CoinSymbol { get; set; }
        public decimal PurchasePrice { get; set; }
        public long PurchaseDate { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}