using System.ComponentModel;

namespace Service.ManagerVPS.Constants.Enums;

public enum UserRoleEnum
{
    [Description("Admin")] 
    ADMIN = 1,
    [Description("Quản lý bãi đỗ xe")]
    OWNER = 2,
    [Description("Bảo vệ")]
    ATTENDANT = 3
}