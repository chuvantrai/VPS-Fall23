using Newtonsoft.Json;
using Service.ManagerVPS.Constants.Enums.VNPay;
using Service.ManagerVPS.DTO.VNPay;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Service.ManagerVPS.ExternalClients.VNPay
{

    public sealed class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
    public static class VNPayHelper
    {
        public static string HmacSHA512(string key, string input)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }
        public static bool ValidateSignature(IQueryCollection queryString, string secretKey)
        {
            string secureHash = queryString[Constant.Key.SecureHash];
            string rspRaw = GetResponseData(queryString);
            string myChecksum = HmacSHA512(secretKey, rspRaw);
            return myChecksum.Equals(secureHash, StringComparison.InvariantCultureIgnoreCase);
        }
        private static string GetResponseData(IQueryCollection queryString)
        {
            SortedList<string, string> sortedResponse = new SortedList<string, string>(new VnPayCompare());
            foreach (var s in queryString)
            {
                if (!string.IsNullOrEmpty(s.Value) && s.Key.StartsWith("vnp_"))
                {
                    sortedResponse.Add(s.Key, s.Value);
                }
            }
            StringBuilder data = new StringBuilder();
            if (sortedResponse.ContainsKey("vnp_SecureHashType"))
            {
                sortedResponse.Remove("vnp_SecureHashType");
            }
            if (sortedResponse.ContainsKey("vnp_SecureHash"))
            {
                sortedResponse.Remove("vnp_SecureHash");
            }
            foreach (KeyValuePair<string, string> kv in sortedResponse)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }
            return data.ToString();
        }
    }
    public class VNPayClient
    {

        private SortedList<string, string> sortedRequestParams = new SortedList<string, string>(new VnPayCompare());
        readonly VnPayConfig vnPayConfig;
        public VNPayClient(VnPayConfig vnPayConfig)
        {
            this.vnPayConfig = vnPayConfig;
            AddDefaultRequestParams();
            sortedRequestParams.Add(Constant.Key.Command, Constant.Value.Command);

        }
        public VNPayClient InitRequestParams(string customerIpAddress, out string transactionId)
        {
            //sortedRequestParams.Add(Constant.Key.BankCode, bankCode.ToString());
            sortedRequestParams.Add(Constant.Key.IpAddress, customerIpAddress);
            transactionId = DateTime.Now.Ticks.ToString();
            sortedRequestParams.Add(Constant.Key.TxnRef, transactionId);
            return this;
        }
        public string CreateRequestUrl(string baseUrl, decimal money, int minuteExpire, out string secureHash, string orderInfo = "")
        {

            sortedRequestParams.Add(Constant.Key.Amount, (money * 100).ToString("0"));
            sortedRequestParams.Add(Constant.Key.OrderInfo, orderInfo);
            DateTime createdAt = DateTime.Now;
            DateTime expire = createdAt.Add(TimeSpan.FromMinutes(minuteExpire));
            sortedRequestParams.Add(Constant.Key.CreateDate, createdAt.ToString("yyyyMMddHHmmss"));
            sortedRequestParams.Add(Constant.Key.ExpireDate, expire.ToString("yyyyMMddHHmmss"));

            return GetBaseUrlWithQueryString(baseUrl,out secureHash);
        }
        void AddDefaultRequestParams()
        {
            var properties = typeof(VnPayConfig).GetProperties();
            foreach (var item in properties)
            {
                if (!item.Name.StartsWith("Vnp_")) continue;
                var jsonName = item.GetCustomAttribute<JsonPropertyAttribute>(false).PropertyName;
                sortedRequestParams.Add(jsonName, item.GetValue(vnPayConfig).ToString());
            }
        }
        StringBuilder GetQueryStringFromRequestParams()
        {
            StringBuilder data = new();
            foreach (KeyValuePair<string, string> kv in sortedRequestParams)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            return data;
        }
        string GetBaseUrlWithQueryString(string baseUrl, out string secureHash)
        {
            var queryString = GetQueryStringFromRequestParams();
            baseUrl = $"{baseUrl}?{queryString}";
            string signData = queryString.ToString();
            if (signData.Length > 0)
            {
                signData = signData.Remove(signData.Length - 1, 1);
            }
            secureHash = VNPayHelper.HmacSHA512(vnPayConfig.HashSecret, signData);
            baseUrl = $"{baseUrl}{Constant.Key.SecureHash}={secureHash}";

            return baseUrl;

        }

    }
}
