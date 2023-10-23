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


        public static string NOTIFICATION = "NOTIFICATION";
        public static string ALERT = "ALERT";
        public static string CANCEL = "CANCEL";
        public static string ALERT_ERROR = "PLEASE TAKE THE LICENSE PLATE IN TO AREA !!!";
    }
}
