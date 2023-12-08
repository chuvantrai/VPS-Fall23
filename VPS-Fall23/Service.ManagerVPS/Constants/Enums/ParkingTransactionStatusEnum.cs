using System.ComponentModel;

namespace Service.ManagerVPS.Constants.Enums
{
    public enum ParkingTransactionStatusEnum
    {
        [Description("Đã Đặt Lịch")]
        BOOKED = 1,
        [Description("Đã Đặt Cọc")]
        DEPOSIT = 2,
        [Description("Chưa Trả Tiền")]
        UNPAY = 12,
        [Description("Người dùng hủy")]
        USERCANCEL = 13,
        [Description("Nhà xe hủy")]
        PARKINGCANCEL = 14,
        [Description("Đã Trả Tiền")]
        PAYED = 16,
        [Description("Thanh toán thất bại")]
        BOOKING_PAID_FAILED = 18
    }
}
