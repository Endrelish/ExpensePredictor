using System;

namespace ExpensePrediction.Exceptions
{
    public class ExpenseException : ServiceException
    {
        private static readonly string ErrorCode = "expense_exception";

        public ExpenseException(int htmlCode) : base(ErrorCode)
        {
            HttpCode = htmlCode;
        }

        public ExpenseException(string message, int htmlCode) : base(ErrorCode, message)
        {
            HttpCode = htmlCode;
        }

        public ExpenseException(string message, Exception inner, int htmlCode) : base(ErrorCode, message, inner)
        {
            HttpCode = htmlCode;
        }

        public override int HttpCode { get; }
    }
}