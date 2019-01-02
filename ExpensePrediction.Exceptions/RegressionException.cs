using System;

namespace ExpensePrediction.Exceptions
{
    public class RegressionException : ServiceException
    {
        private static readonly string ErrorCode = "regression_exception";

        public RegressionException()
            : base(ErrorCode)
        {
        }

        public RegressionException(string message)
            : base(ErrorCode, message)
        {
        }

        public RegressionException(string message, Exception inner)
            : base(ErrorCode, message, inner)
        {
        }

        public override int HttpCode { get; } = 500;
    }
}