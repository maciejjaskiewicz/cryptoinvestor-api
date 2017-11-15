using CryptoInvestor.Infrastructure.Commands.Auth;

namespace CryptoInvestor.Infrastructure.Commands.Account
{
    public class UpdateUser : AuthenticatedCommandBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
    }
}
