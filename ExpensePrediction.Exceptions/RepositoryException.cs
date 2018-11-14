namespace ExpensePrediction.Exceptions
{
    using System;

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