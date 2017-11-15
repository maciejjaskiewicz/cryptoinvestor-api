using System.Collections.Generic;

namespace CryptoInvestor.Tests.Extensions
{
    public static class TestData
    {
        public static IEnumerable<string> ValidEmails => _validEmails;
        public static IEnumerable<string> InvalidEmails => _invalidEmails;

        private static ISet<string> _validEmails = new HashSet<string>
        {
            "user1@email.com",
            "email@domain.com",
            "firstname.lastname@domain.com",
            "email@subdomain.domain.com",
            "firstname+lastname@domain.com",
            "1234567890@domain.com",
            "email@domain-one.com",
            "email@domain.name",
            "email@domain.co.jp",
            "firstname-lastname@domain.com",
            "j_9@[129.126.118.1]",
            "js@proseware.com9",
            "\"j\\\"s\\\"\"@proseware.com"
        };

        private static ISet<string> _invalidEmails = new HashSet<string>
        {
            "plainaddress",
            "#@%^%#$@#$@#.com",
            "@domain.com",
            "Joe Smith <email@domain.com>",
            "email.domain.com",
            "email@domain@domain.com",
            ".email@domain.com",
            "email.@domain.com",
            "email..email@domain.com",
            "あいうえお@domain.com",
            "email@domain.com (Joe Smith)",
            "email@domain",
            "email@-domain.com",
            "email@domain..com",
            "js*@proseware.com"
        };
    }
}