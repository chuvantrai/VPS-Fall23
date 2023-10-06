namespace Service.ManagerVPS.DTO.Exceptions
{
    public class ForbidenException : VpsException
    {
        public ForbidenException() { }
        public ForbidenException(string message, Exception? innerException = null) : base(message, innerException) { }
    }
}
