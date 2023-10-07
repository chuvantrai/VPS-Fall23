using Service.ManagerVPS.Constants.Notifications;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Service.ManagerVPS.DTO.Exceptions
{
    public abstract class Error
    {
        private static Error? instance;

        public static Error Instance
        {
            get
            {
                instance ??= new ErrorWithFile();
                return instance;
            }
        }

        public abstract string GetErrorMessage(int code);
    }

    public class ErrorWithFile : Error
    {
        public class ErrorModel
        {
            [JsonPropertyName("code")] public int Code { get; set; }
            [JsonPropertyName("message")] public string Message { get; set; } = null!;
        }

        static readonly string exceptionsStorePath = $"{Directory.GetCurrentDirectory()}/exceptions.json";
        static DateTime lastModifiedErrorFile;
        static ICollection<ErrorModel>? errors;

        ICollection<ErrorModel>? Errors
        {
            get
            {
                try
                {
                    DateTime lastModifiedErrorFile = File.GetLastWriteTime(exceptionsStorePath);
                    bool isNeedUpdate = errors == null || lastModifiedErrorFile > ErrorWithFile.lastModifiedErrorFile;
                    if (isNeedUpdate)
                    {
                        ErrorWithFile.lastModifiedErrorFile = lastModifiedErrorFile;
                        using FileStream fileReadStream = new(exceptionsStorePath, FileMode.Open, FileAccess.Read);
                        errors = JsonSerializer.Deserialize<List<ErrorModel>>(fileReadStream);
                    }

                    return errors;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public override string GetErrorMessage(int code)
        {
            if (Errors is null || Errors.Count == 0) return ResponseNotification.SERVER_ERROR;
            bool predicate(ErrorModel errorModel) => errorModel.Code == code;
            bool isContainCode = Errors.Any(predicate);
            if (isContainCode)
            {
                return Errors.FirstOrDefault(predicate)!.Message;
            }

            return ResponseNotification.SERVER_ERROR;
        }
    }
}