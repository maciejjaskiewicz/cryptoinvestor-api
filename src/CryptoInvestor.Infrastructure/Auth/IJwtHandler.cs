using CryptoInvestor.Infrastructure.DTO;
using System;

namespace CryptoInvestor.Infrastructure.Auth
{
    public interface IJwtHandler
    {
        JwtDto CreateToken(Guid userId, string email);
    }
}