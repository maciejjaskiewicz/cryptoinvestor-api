using System;

namespace CryptoInvestor.Core.Exceptions
{
    public abstract class CryptoInvestorException : Exception
    {
        public string Code { get; }

        protected CryptoInvestorException()
        {
        }

        protected CryptoInvestorException(string code)
        {
            Code = code;
        }

        protected CryptoInvestorException(string message, params object[] args)
            : this(string.Empty, message, args)
        {
        }

        protected CryptoInvestorException(string code, string message, params object[] args)
            : this(null, code, message, args)
        {
        }

        protected CryptoInvestorException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {
        }

        protected CryptoInvestorException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}