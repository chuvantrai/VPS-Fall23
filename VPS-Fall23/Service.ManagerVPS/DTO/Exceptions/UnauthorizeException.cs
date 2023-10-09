namespace Service.ManagerVPS.DTO.Exceptions
{
    public class UnauthorizeException : VpsException
    {
        public UnauthorizeException(int code, Exception? innerException = null)
            : base(code, innerException)
        {
        }

        public UnauthorizeException(Exception? innerException = null)
            : base(4, innerException)
        {
        }

        public UnauthorizeException(string message, Exception? innerException = null)
            : base(4, message, innerException)
        {
        }
    }
}