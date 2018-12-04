using System;

namespace ExpensePrediction.Exceptions
{
    public class ExpressionNullException : RepositoryException
    {
        private static readonly string ErrorCode = "expression_null";

        public ExpressionNullException()
            : base(ErrorCode)
        {
        }

        public ExpressionNullException(Exception inner)
            : base(ErrorCode, inner)
        {
        }
    }
}