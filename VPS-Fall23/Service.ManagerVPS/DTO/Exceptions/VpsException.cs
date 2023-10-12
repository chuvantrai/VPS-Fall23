using System.Text.Json;

namespace Service.ManagerVPS.DTO.Exceptions
{
    [Serializable]
    public abstract class VpsException : Exception
    {
        public int Code { get; set; }

        public VpsException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
            Code = 1;
        }

        public VpsException(int code, string message, Exception? innerException = null)
            : base(message, innerException)
        {
            Code = code;
        }

        public VpsException(int code, Exception? innerException = null)
            : this(code, Error.Instance.GetErrorMessage(code), innerException)
        {
        }
    }
}