namespace Service.ManagerVPS.DTO.Exceptions
{
    public class UnAuthorizeException : VpsException
    {
        public UnAuthorizeException() { }
        public UnAuthorizeException(string message, Exception? innerException = null) : base(message, innerException) { }
    }
}
