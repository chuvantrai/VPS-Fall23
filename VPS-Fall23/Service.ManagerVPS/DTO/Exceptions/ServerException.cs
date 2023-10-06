namespace Service.ManagerVPS.DTO.Exceptions
{

    [Serializable]
    public class ServerException : VpsException
    {
        public ServerException() { }
        public ServerException(string message, Exception? innerException = null) : base(message, innerException) { }
    }
}
