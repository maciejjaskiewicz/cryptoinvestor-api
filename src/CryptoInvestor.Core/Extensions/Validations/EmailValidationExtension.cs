using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CryptoInvestor.Core.Extensions.Validations
{
    public static class EmailValidationExtension
    {
        private static bool invalid = false;

        private static readonly Regex EmailRegex = new Regex(
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250)
        );

        public static bool IsValidEmail(this string email)
        {
            invalid = false;

            try
            {
                email = Regex.Replace(
                    email,
                    @"(@)(.+)$",
                    DomainMapper,
                    RegexOptions.None,
                    TimeSpan.FromMilliseconds(200)
                );
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid) return false;

            try
            {
                return EmailRegex.IsMatch(email);
            }
            catch
            {
                return false;
            }
        }

        private static string DomainMapper(Match match)
        {
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
    }
}