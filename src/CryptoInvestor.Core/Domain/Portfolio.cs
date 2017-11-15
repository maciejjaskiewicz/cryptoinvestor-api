using CryptoInvestor.Core.Exceptions;
using CryptoInvestor.Core.Extensions.Validations;
using System;

namespace CryptoInvestor.Core.Domain
{
    public class Portfolio
    {
        public Guid Id { get; protected set; }
        public Guid UserId { get; protected set; }
        public string NameId { get; protected set; }
        public string Name { get; protected set; }
        public long CreatedAt { get; private set; }

        protected Portfolio()
        {
        }

        public Portfolio(Guid userId, string name)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            SetName(name);
            CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public void SetName(string name)
        {
            if (name.Empty())
            {
                throw new DomainException(ErrorCodes.InvalidPortfolioName,
                    "Portfolio name can not be empty.");
            }

            Name = name;
            SetNameId(name);
        }

        public void SetNameId(string name)
        {
            var nameId = name.ToLowerInvariant().Trim().Replace(' ', '-');
            NameId = nameId;
        }
    }
}