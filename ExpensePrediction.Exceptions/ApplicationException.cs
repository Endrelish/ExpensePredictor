namespace ExpensePrediction.Exceptions
{
    using System;

    public abstract class ApplicationException : Exception
    {
        public string Code { get; }
        public abstract int HttpCode { get; }
        protected ApplicationException(string code)
        {
            Code = code;
        }

        protected ApplicationException(string code, string message)
            : base(message)
        {
            Code = code;
        }

        protected ApplicationException(string code, string message, Exception inner)
            : base(message, inner)
        {
            Code = code;
        }
    }
}