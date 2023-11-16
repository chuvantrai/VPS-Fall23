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
        public const string NOTIFICATION = "THÔNG BÁO";
        public const string ALERT = "CẢNH BÁO";
        public const string CANCEL = "Thoát";
        public const string ACCEPT = "Xác nhận trả";
        public const string ALERT_ERROR = "Hãy đưa biển số xe vào đúng khu vực quét !!!";
        public const string LOGIN_SUCCESS = "Đăng nhập thành công !";
        public const string INPUT_FAILED = "Chưa nhập biển số xe vào chỗ trống !";
        public const string LOGIN_FAILED = "Mật khẩu hoặc tài khoản chưa chính xác !";
        public const string CHECKOUT_CONFIRM = "Chưa trả tiền xe";
        public const string OVERTIME_CONFIRM = "Số tiền bạn phải trả là : ";
    }
}
