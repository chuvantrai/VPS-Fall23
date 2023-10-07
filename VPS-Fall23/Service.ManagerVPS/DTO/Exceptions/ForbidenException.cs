namespace Service.ManagerVPS.DTO.Exceptions
{
    public class ForbidenException : VpsException
    {
        public ForbidenException(int code, Exception? innerException = null)
              : base(code, innerException)
        {

        }
        public ForbidenException(Exception? innerException = null)
            : base(5, Error.Instance.GetErrorMessage(3), innerException)
        {

        }
        public ForbidenException(string message, Exception? innerException = null)
            : base(5, message, innerException)
        {
        }
    }
}
