namespace Service.ManagerVPS.Extensions.ILogic;

public interface IVnPayLibrary
{
    string GetVersionVnPay();
    void AddRequestData(string key, string value);
    void AddResponseData(string key, string value);
    string GetResponseData(string key);
    string CreateRequestUrl(string baseUrl, string vnp_HashSecret);
    bool ValidateSignature(string inputHash, string secretKey);
}