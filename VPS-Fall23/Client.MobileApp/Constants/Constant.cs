namespace Client.MobileApp.Constants
{
    public static class Constant
    {
        public static string GoogleAppCredentials = "GoogleAppCredentials.json";
        public static string ImageName = "Image " + DateTime.Now.ToString();
        public static string API_PATH_VPS53 = "api/ParkingTransaction/CheckLicensePlateScan";
        public static string API_PATH_VPS61 = "api/ParkingTransaction/CheckLicensePlateInput";
        public static string API_PATH_VPS79 = "api/Auth/AttendanceLogin";
        public static string API_PATH_VPS80_1 = "api/ParkingTransaction/CheckOutScanConfirm";
        public static string API_PATH_VPS80_2 = "api/ParkingTransaction/CheckOutInputConfirm";
        public static Guid USER = new Guid();

        /// <summary>
        /// Notification Message
        /// </summary>
        public static string NOTIFICATION = "THÔNG BÁO";
        public static string ALERT = "CẢNH BÁO";
        public static string CANCEL = "Thoát";
        public static string ACCEPT = "Xác nhận trả";
        public static string ALERT_ERROR = "Hãy đưa biển số xe vào đúng khu vực quét !!!";
        public static string LOGIN_SUCCESS = "Đăng nhập thành công !";
        public static string LOGIN_FAILED = "Mật khẩu hoặc tài khoản chưa chính xác !";
        public static string CHECKOUT_CONFIRM = "Chưa trả tiền xe";
        public const string OVERTIME_CONFIRM = "Số tiền bạn phải trả là : ";
    }
}
