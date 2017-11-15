using CryptoInvestor.Infrastructure.Auth;
using Shouldly;
using System;
using Xunit;

namespace CryptoInvestor.Tests.Auth
{
    public class EncrypterTest
    {
        private readonly IEncrypter encrypter;

        public EncrypterTest()
        {
            encrypter = new Encrypter();
        }

        [Fact]
        public void Given_valid_password_GetSalt_should_generate_salt()
        {
            var password = "secret123";
            var salt = encrypter.GetSalt(password);
            salt.ShouldNotBeEmpty();
        }

        [Fact]
        public void Given_invalid_password_GetSalt_should_throw_exception()
        {
            var password = "";

            Should.Throw<ArgumentException>(() =>
            {
                encrypter.GetSalt(password);
            });
        }

        [Fact]
        public void GetSalt_should_generate_different_salts()
        {
            var password = "secret123";

            var salt1 = encrypter.GetSalt(password);
            var salt2 = encrypter.GetSalt(password);

            salt1.ShouldNotBe(salt2);
        }

        [Fact]
        public void Given_valid_password_and_salt_GetHash_should_generate_hash()
        {
            var password = "secret123";
            var salt = encrypter.GetSalt(password);

            var hash = encrypter.GetHash(password, salt);

            hash.ShouldNotBeEmpty();
        }

        [Fact]
        public void Given_invalid_password_GetHash_should_throw_exception()
        {
            var password = "secret123";
            var salt = encrypter.GetSalt(password);

            Should.Throw<ArgumentException>(() =>
            {
                encrypter.GetHash("", salt);
            });
        }

        [Fact]
        public void Given_invalid_salt_GetHash_should_throw_exception()
        {
            var password = "secret123";
            var salt = "";

            Should.Throw<ArgumentException>(() =>
            {
                encrypter.GetHash(password, salt);
            });
        }

        [Fact]
        public void GetHash_should_generate_the_same_hash()
        {
            var password = "secret123";
            var salt = encrypter.GetSalt(password);

            var hash1 = encrypter.GetHash(password, salt);
            var hash2 = encrypter.GetHash(password, salt);

            hash1.ShouldBe(hash2);
        }
    }
}