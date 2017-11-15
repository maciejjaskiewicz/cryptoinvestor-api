using CryptoInvestor.Core.Exceptions;
using System;

namespace CryptoInvestor.Infrastructure.Exceptions
{
    public class ServiceException : CryptoInvestorException
    {
        public ServiceException()
        {
        }

        public ServiceException(string code) : base(code)
        {
        }

        public ServiceException(string message, params object[] args)
            : base(string.Empty, message, args)
        {
        }

        public ServiceException(string code, string message, params object[] args)
            : base(null, code, message, args)
        {
        }

        public ServiceException(Exception innerException, string message, params object[] args)
            : base(innerException, string.Empty, message, args)
        {
        }

        public ServiceException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
        }
    }
}