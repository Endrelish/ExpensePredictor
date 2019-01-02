using System;

namespace ExpensePrediction.Exceptions
{
    public abstract class RepositoryException : ExpensePrediction.Exceptions.ApplicationException
    {
        protected RepositoryException(string code)
            : base(code)
        {
        }
        protected RepositoryException(string code, string message)
            : base(code, message)
        {
        }

        protected RepositoryException(string code, string message, Exception inner)
            : base(code, message, inner)
        {
        }
    }
}