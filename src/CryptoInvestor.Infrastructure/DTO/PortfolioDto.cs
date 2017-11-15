using System;

namespace CryptoInvestor.Infrastructure.DTO
{
    public class PortfolioDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string NameId { get; set; }
        public string Name { get; set; }
        public long CreatedAt { get; set; }
    }
}