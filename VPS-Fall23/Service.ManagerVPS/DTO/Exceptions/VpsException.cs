namespace Service.ManagerVPS.DTO.Exceptions
{

    [Serializable]
    public class VpsException : Exception
    {
        public VpsException() { }
        public VpsException(string message) : base(message) { }
        public VpsException(string message, Exception? inner = null) : base(message, inner) { }
        protected VpsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
