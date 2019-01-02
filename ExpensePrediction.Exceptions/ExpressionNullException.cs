using System;

namespace ExpensePrediction.Exceptions
{
    public class ExpressionNullException : RepositoryException
    {
        private static readonly string ErrorCode = "expression_null_exception";

        public ExpressionNullException()
            : base(ErrorCode)
        {
        }

        public ExpressionNullException(string message)
            : base(ErrorCode, message)
        {
        }

        public ExpressionNullException(string message, Exception inner)
            : base(ErrorCode, message, inner)
        {
        }

        public override int HttpCode { get; } = 500;
    }
}