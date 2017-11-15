using System;
using System.Collections.Generic;

namespace CryptoInvestor.Infrastructure.DTO
{
    public class FavouritesDto
    {
        public Guid Id { get; set; }
        public IEnumerable<CoinDto> Coins { get; set; }
    }
}