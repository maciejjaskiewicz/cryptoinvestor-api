﻿namespace CryptoInvestor.Infrastructure.Auth
{
    public interface IEncrypter
    {
        string GetSalt(string value);
        string GetHash(string value, string salt);
    }
}
