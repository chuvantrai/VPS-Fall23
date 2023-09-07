using Service.ManagerCRM.Constants.Enums;

namespace Service.ManagerCRM.ModelsNoneDB.OtherModels;

public class ActionModel
{
    public ActionFilterEnum Action { get; set; }
    public UserRoleEnum[]? UserRole { get; set; }
}