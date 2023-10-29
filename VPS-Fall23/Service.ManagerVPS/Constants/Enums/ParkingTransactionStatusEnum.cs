using System.ComponentModel;

namespace Service.ManagerVPS.Constants.Enums
{
    public enum ParkingTransactionStatusEnum
    {
        [Description("Đã Đặt Lịch")]
        BOOKED = 1,
        [Description("Đã Đặt Cọc")]
        DEPOSIT = 2,
        [Description("Người dùng hủy")]
        USERCANCEL = 3,
        [Description("Nhà xe hủy")]
        PARKINGCANCEL = 4,
        [Description("Thanh toán thất bại")]
        BOOKING_PAID_FAILED = 5
    }
}
