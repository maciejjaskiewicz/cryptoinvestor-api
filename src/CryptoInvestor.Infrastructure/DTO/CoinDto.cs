using System.Collections.Generic;

namespace CryptoInvestor.Infrastructure.DTO
{
    public class CoinDto
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public IEnumerable<CoinPriceDto> Prices { get; set; }
        public decimal MarketCap { get; set; }
        public decimal Change24h { get; set; }
        public decimal Volume24h { get; set; }
        public string IconUrl { get; set; }
    }
}