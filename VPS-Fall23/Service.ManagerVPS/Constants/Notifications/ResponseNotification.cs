namespace Service.ManagerVPS.Constants.Notifications;

public static class ResponseNotification
{
    /*
     * Notification Api
     */ 
    public const string SERVER_ERROR = "Lỗi hệ thống";
    public const string NOT_FOUND = "Không tìm thấy nội dung yêu cầu";
    public const string CLIENT_ERROR = "Có lỗi xảy ra, vui lòng kiểm tra lại";

    /*
     * Notification Api success
     */
    public const string ADD_SUCCESS = "Tạo thành công";
    public const string UPDATE_SUCCESS = "Cập nhật thành công";
    public const string DELETE_SUCCESS = "Xóa thành công";
    public const string GET_SUCCESS = "Lấy thành công";
    
    /*
     * Notification Api error
     */
    public const string ADD_ERROR = "Tạo thất bại";
    public const string UPDATE_ERROR = "Cập nhật thất bại";
    public const string DELETE_ERROR = "Xóa thất bại";
    public const string GET_ERROR = "Lấy thất bại";

    /*
     * Notification License Plate
     */
    public const string CHECKIN_SUCCESS = "Checkin thành công";
    public const string CHECKIN_ERROR = "Checkin thất bại";
    public const string CHECKOUT_SUCCESS = "Checkout thành công";
    public const string CHECKOUT_ERROR = "Checkout thất bại";
    public const string CHECKOUT_CONFIRM = "Chưa trả tiền xe";
    public const string NO_DATA = "Không có dữ liệu";
    public const string CONFIRM_ERROR = "Xác nhận thất bại";
    public const string CONFIRM_SUCCESS = "Xác nhận thành công";
    public const string BOOKING_ERROR = "Bãi đỗ xe đã hết chỗ";
    public const string OVERTIME_CONFIRM = "Số tiền bạn phải trả là : ";
}