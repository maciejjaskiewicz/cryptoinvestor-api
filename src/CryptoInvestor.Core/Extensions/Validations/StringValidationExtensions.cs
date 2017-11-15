using System;
using System.Text.RegularExpressions;

namespace CryptoInvestor.Core.Extensions.Validations
{
    public static class StringValidationExtensions
    {
        private static readonly Regex InvalidCharactersRegex = new Regex(
            @"[^\w\.@-]",
            RegexOptions.Compiled,
            TimeSpan.FromSeconds(1.5)
        );

        public static bool Empty(this string value) =>
            string.IsNullOrWhiteSpace(value);

        public static bool NotEmpty(this string value) =>
            !value.Empty();

        public static bool HasInvalidCharacters(this string value)
        {
            try
            {
                return InvalidCharactersRegex.IsMatch(value);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}