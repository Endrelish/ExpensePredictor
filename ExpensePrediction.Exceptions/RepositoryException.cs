using System;

namespace ExpensePrediction.Exceptions
{
    public class RepositoryException : Exception
    {
        public RepositoryException(string errorCode)
            : base(errorCode)
        {
        }

        public RepositoryException(string errorCode, Exception inner)
            : base(errorCode, inner)
        {
        }
    }
}