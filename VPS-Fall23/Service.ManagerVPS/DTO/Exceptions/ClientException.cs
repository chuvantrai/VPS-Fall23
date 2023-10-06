namespace Service.ManagerVPS.DTO.Exceptions
{

    [Serializable]
    public class ClientException : VpsException
    {
        public ClientException() { }
        public ClientException(string message, Exception? innerException = null) : base(message, innerException) { }

    }
}
