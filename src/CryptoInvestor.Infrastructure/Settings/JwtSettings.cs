namespace CryptoInvestor.Infrastructure.Settings
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Key { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}
