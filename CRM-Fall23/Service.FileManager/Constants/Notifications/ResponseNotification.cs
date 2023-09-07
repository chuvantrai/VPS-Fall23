namespace Service.FileManager.Constants.Notifications;

public class ResponseNotification
{
    /*
     * Notification Api
     */ 
    public const string SERVER_ERROR = "Lỗi hệ thống";
    
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
     * Other
     */
    public const string OVER_SIZE = "Vượt quá dung lượng cho phép";
}