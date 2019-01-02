using System;

namespace ExpensePrediction.Exceptions
{
    public class AccountException : ServiceException
    {
        private static readonly string ErrorCode = "account_exception";

        public AccountException(int htmlCode) : base(ErrorCode)
        {
            HttpCode = htmlCode;
        }

        public AccountException(string message, int htmlCode) : base(ErrorCode, message)
        {
            HttpCode = htmlCode;
        }

        public AccountException(string message, Exception inner, int htmlCode) : base(ErrorCode, message, inner)
        {
            HttpCode = htmlCode;
        }

        public override int HttpCode { get; }
    }
}