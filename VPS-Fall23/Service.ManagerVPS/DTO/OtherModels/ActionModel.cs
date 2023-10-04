using Service.ManagerVPS.Constants.Enums;

namespace Service.ManagerVPS.DTO.OtherModels;

public class ActionModel
{
    public ActionFilterEnum Action { get; set; }

    public UserRoleEnum[]? UserRole { get; set; }
}