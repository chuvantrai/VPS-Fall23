using System.ComponentModel;

namespace Service.ManagerVPS.Constants.Enums;

public enum UserRoleEnum
{
    [Description("Admin")] 
    ADMIN = 1,
    [Description("Khách hàng")]
    CUSTUMER = 2
}