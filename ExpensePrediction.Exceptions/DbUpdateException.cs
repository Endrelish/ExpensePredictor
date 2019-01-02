using System;

namespace ExpensePrediction.Exceptions
{
    public class DbUpdateException : RepositoryException
    {
        private static readonly string ErrorCode = "db_update_exception";

        public DbUpdateException()
            : base(ErrorCode)
        {
        }

        public DbUpdateException(string message)
            : base(ErrorCode, message)
        {
        }

        public DbUpdateException(string message, Exception inner)
            : base(ErrorCode, message, inner)
        {
        }

        public override int HttpCode { get; } = 500;
    }
}