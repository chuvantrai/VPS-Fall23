namespace Service.ManagerVPS.DTO.Exceptions
{
    [Serializable]
    public class ServerException : VpsException
    {
        public ServerException(int code, Exception? innerException = null)
            : base(code, innerException)
        {
        }

        public ServerException(Exception? innerException = null)
            : base(1, Error.Instance.GetErrorMessage(1), innerException)
        {
        }

        public ServerException(string message, Exception? innerException = null)
            : base(1, message, innerException)
        {
        }
    }
}