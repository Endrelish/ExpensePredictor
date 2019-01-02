namespace ExpensePrediction.Exceptions
{
    using System;

    public class RestException : Exception
    {
        public string Code { get; }
        public RestException(string code)
        {
            Code = code;
        }

        public RestException(string message, string code)
            : base(message)
        {
            Code = code;
        }

        public RestException(string message, string code, Exception inner)
            : base(message, inner)
        {
            Code = code;
        }
    }
}