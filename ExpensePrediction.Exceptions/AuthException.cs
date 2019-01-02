using System;

namespace ExpensePrediction.Exceptions
{
    public class AuthException : ServiceException
    {
        private static readonly string ErrorCode = "account_exception";

        public AuthException(int htmlCode) : base(ErrorCode)
        {
            HttpCode = htmlCode;
        }

        public AuthException(string message, int htmlCode) : base(ErrorCode, message)
        {
            HttpCode = htmlCode;
        }

        public AuthException(string message, Exception inner, int htmlCode) : base(ErrorCode, message, inner)
        {
            HttpCode = htmlCode;
        }

        public override int HttpCode { get; }
    }
}