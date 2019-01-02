
using System;

namespace ExpensePrediction.Exceptions
{
    public abstract class ServiceException : ExpensePrediction.Exceptions.ApplicationException
    {
        protected ServiceException(string code)
            : base(code)
        {
        }

        protected ServiceException(string code, string message)
            : base(code, message)
        {
        }

        protected ServiceException(string code, string message, Exception inner)
            : base(code, message, inner)
        {
        }
    }
}