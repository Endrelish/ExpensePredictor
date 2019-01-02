using System;

namespace ExpensePrediction.Exceptions
{
    public class CategoryException : ServiceException
    {
        private static readonly string ErrorCode = "account_exception";

        public CategoryException(int htmlCode) : base(ErrorCode)
        {
            HttpCode = htmlCode;
        }

        public CategoryException(string message, int htmlCode) : base(ErrorCode, message)
        {
            HttpCode = htmlCode;
        }

        public CategoryException(string message, Exception inner, int htmlCode) : base(ErrorCode, message, inner)
        {
            HttpCode = htmlCode;
        }

        public override int HttpCode { get; }
    }
}