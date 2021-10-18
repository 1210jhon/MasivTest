using System;

namespace Rest.API.Infrastructure.Exceptions
{
    public class RouletteDomainException : Exception
    {
        private int errorId;

        public int ErrorId => errorId;

        public RouletteDomainException()
        { }

        public RouletteDomainException(string message)
            : base(message)
        { }

        public RouletteDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public RouletteDomainException(string message, int errorId)
            : base(message)
        {
            this.errorId = errorId;
        }
    }
}
