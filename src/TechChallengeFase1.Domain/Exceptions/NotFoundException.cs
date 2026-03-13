using System;
using System.Collections.Generic;
using System.Text;

namespace TechChallengeFase1.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {

        }

        public NotFoundException(string message)
                : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
