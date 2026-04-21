namespace TechChallengeFase1.Domain.Exceptions
{
    public class ExternalServiceException : Exception
    {
        public ExternalServiceException(string message) : base(message)
        {
        }
    }
}
