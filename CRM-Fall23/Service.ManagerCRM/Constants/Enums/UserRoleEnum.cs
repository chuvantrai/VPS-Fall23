using System.ComponentModel;

namespace Service.ManagerCRM.Constants.Enums;

public enum UserRoleEnum
{
    [Description("Admin")] 
    ADMIN = 1,
    [Description("Khách hàng")]
    CUSTUMER = 2
}