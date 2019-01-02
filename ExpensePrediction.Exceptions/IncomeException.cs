using System;

namespace ExpensePrediction.Exceptions
{
    public class IncomeException : ServiceException
    {
        private static readonly string ErrorCode = "account_exception";

        public IncomeException(int htmlCode) : base(ErrorCode)
        {
            HttpCode = htmlCode;
        }

        public IncomeException(string message, int htmlCode) : base(ErrorCode, message)
        {
            HttpCode = htmlCode;
        }

        public IncomeException(string message, Exception inner, int htmlCode) : base(ErrorCode, message, inner)
        {
            HttpCode = htmlCode;
        }

        public override int HttpCode { get; }
    }
}