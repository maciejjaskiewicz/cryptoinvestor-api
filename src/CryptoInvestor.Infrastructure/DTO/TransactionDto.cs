using System;

namespace CryptoInvestor.Infrastructure.DTO
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string PortfolioNameId { get; set; }
        public CoinDto Coin { get; set; }
        public decimal PurchasePrice { get; set; }
        public long PurchaseDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Profit { get; set; }
        public string Currency { get; set; }
        public bool IsSold { get; set; }
        public decimal SoldPrice { get; set; }
        public long SoldDate { get; set; }
    }
}