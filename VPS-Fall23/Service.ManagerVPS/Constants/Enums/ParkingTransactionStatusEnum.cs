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
        UNPAY = 3,
        [Description("Người dùng hủy")]
        USERCANCEL = 4,
        [Description("Nhà xe hủy")]
        PARKINGCANCEL = 5,
        [Description("Đã Trả Tiền")]
        PAYED = 6,
    }
}
