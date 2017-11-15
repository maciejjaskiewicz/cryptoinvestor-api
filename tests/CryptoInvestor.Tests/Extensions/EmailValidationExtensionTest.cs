using CryptoInvestor.Core.Extensions.Validations;
using Shouldly;
using Xunit;

namespace CryptoInvestor.Tests.Extensions
{
    public class EmailValidationExtensionTest
    {
        [Fact]
        public void When_calling_IsValidEmail_and_email_is_valid_it_should_return_true()
        {
            foreach (var email in TestData.ValidEmails)
            {
                email.IsValidEmail().ShouldBe(true);
            }
        }

        [Fact]
        public void When_calling_IsValidEmail_and_email_is_invalid_it_should_return_false()
        {
            foreach (var email in TestData.InvalidEmails)
            {
                email.IsValidEmail().ShouldBe(false);
            }
        }
    }
}