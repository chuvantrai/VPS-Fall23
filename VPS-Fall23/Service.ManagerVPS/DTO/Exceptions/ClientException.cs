namespace Service.ManagerVPS.DTO.Exceptions
{
    [Serializable]
    public class ClientException : VpsException
    {
        public ClientException(int code, Exception? innerException = null)
            : base(code, innerException)
        {
        }

        public ClientException(Exception? innerException = null)
            : base(3, Error.Instance.GetErrorMessage(3), innerException)
        {
        }

        public ClientException(string message, Exception? innerException = null)
            : base(3, message, innerException)
        {
        }
    }
}