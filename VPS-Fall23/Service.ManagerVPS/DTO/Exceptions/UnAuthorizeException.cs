namespace Service.ManagerVPS.DTO.Exceptions
{
    public class UnAuthorizeException : VpsException
    {
        public UnAuthorizeException(int code, Exception? innerException = null)
               : base(code, innerException)
        {

        }
        public UnAuthorizeException(Exception? innerException = null)
            : base(4, Error.Instance.GetErrorMessage(4), innerException)
        {

        }
        public UnAuthorizeException(string message, Exception? innerException = null)
            : base(4, message, innerException)
        {
        }
    }
}
