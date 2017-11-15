using System;

namespace CryptoInvestor.Infrastructure.DTO
{
    public class UserDetailsDto : UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public long UpdatedAt { get; set; }
    }
}