using CryptoInvestor.Core.Domain;
using CryptoInvestor.Core.Exceptions;
using Shouldly;
using Xunit;

namespace CryptoInvestor.Tests.Domain
{
    public class UserTest
    {
        private User user;

        public UserTest()
        {
            user = new User("test@email.com", "test", "secret", "salt");
        }

        [Fact]
        public void Given_valid_email_user_email_should_be_changed()
        {
            var newEmail = "test1@email.com";
            user.SetEmail(newEmail);
            user.Email.ShouldBe(newEmail);
        }

        [Fact]
        public void Given_invalid_email_SetEmail_should_throw_proper_exception()
        {
            var newEmail = "testemail.com";

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                user.SetEmail(newEmail);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidEmail);
        }

        [Fact]
        public void Given_valid_username_should_be_changed()
        {
            var newUsername = "test1";
            user.SetUsername(newUsername);
            user.Username.ShouldBe(newUsername);
        }

        [Fact]
        public void Given_invalid_username_SetUsername_should_throw_proper_exception()
        {
            var newUsername = "t";

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                user.SetUsername(newUsername);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidUsername);
        }

        [Fact]
        public void Given_valid_password_and_salt_should_be_changed()
        {
            var newPassword = "secret123";
            var newSalt = "salt";

            user.SetPassword(newPassword, newSalt);

            user.Password.ShouldBe(newPassword);
            user.Salt.ShouldBe(newSalt);
        }

        [Fact]
        public void Given_invalid_password_SetPassword_should_throw_proper_exception()
        {
            var newPassword = "s";
            var newSalt = "salt";

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                user.SetPassword(newPassword, newSalt);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidPassword);
        }

        [Fact]
        public void Given_empty_salt_SetPassword_should_throw_proper_exception()
        {
            var newPassword = "secret123";
            var newSalt = "";

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                user.SetPassword(newPassword, newSalt);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidPassword);
        }

        [Fact]
        public void Given_valid_firstname_should_be_changed()
        {
            var newFirstname = "Jan";
            user.SetFirstName(newFirstname);
            user.FirstName.ShouldBe(newFirstname);
        }

        [Fact]
        public void Given_invalid_firstname_SetFirstname_should_throw_proper_exception()
        {
            var newFirstname = "J";

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                user.SetFirstName(newFirstname);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidFirstName);
        }

        [Fact]
        public void Given_valid_lastname_should_be_changed()
        {
            var newLastname = "Kowalski";
            user.SetLastName(newLastname);
            user.LastName.ShouldBe(newLastname);
        }

        [Fact]
        public void Given_invalid_lastname_SetLastname_should_throw_proper_exception()
        {
            var newLastname = "K";

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                user.SetLastName(newLastname);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidLastName);
        }

        [Fact]
        public void Given_valid_gender_should_be_changed()
        {
            var newGender = "male";
            user.SetGender(newGender);
            user.Gender.ShouldBe(newGender);
        }

        [Fact]
        public void Given_invalid_gender_SetGender_should_throw_proper_exception()
        {
            var newGender = "";

            DomainException ex = Should.Throw<DomainException>(() =>
            {
                user.SetGender(newGender);
            });

            ex.Code.ShouldBe(ErrorCodes.InvalidGender);
        }
    }
}