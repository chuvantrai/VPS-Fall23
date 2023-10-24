using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MobileApp.Constants
{
    public static class Constant
    {
        public static string GoogleAppCredentials = "GoogleAppCredentials.json";
        public static string ImageName = "Image " + DateTime.Now.ToString();
        public static string API_PATH_VPS53 = "api/ParkingTransaction/CheckLicensePlateScan";
        public static string API_PATH_VPS61 = "api/ParkingTransaction/CheckLicensePlateInput";


        public static string NOTIFICATION = "THÔNG BÁO";
        public static string ALERT = "CẢNH BÁO";
        public static string CANCEL = "THOÁT";
        public static string ALERT_ERROR = "HÃY ĐƯA BIỂN SỐ XE VÀO ĐÚNG KHU VỰC QUÉT !!!";
    }
}
