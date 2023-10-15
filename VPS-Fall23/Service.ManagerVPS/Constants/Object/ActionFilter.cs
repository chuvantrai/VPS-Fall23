using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.DTO.OtherModels;

namespace Service.ManagerVPS.Constants.Object;

public static class ActionFilter
{
    private static List<ActionModel> AllActionModel { get; } = new List<ActionModel>()
    {
        new ActionModel()
        {
            Action = ActionFilterEnum.GetOk,
            UserRole = null
        },
        new ActionModel()
        {
            Action = ActionFilterEnum.AddUser,
            UserRole = new[] { UserRoleEnum.ADMIN }
        },
        new ActionModel()
        {
            Action = ActionFilterEnum.TestAuthApi,
            UserRole = new[] { UserRoleEnum.ADMIN }
        },
        new ActionModel()
        {
            Action = ActionFilterEnum.CreateAccountDemo,
            UserRole = null
        },
        new ActionModel()
        {
            Action = ActionFilterEnum.ChangePassword,
            UserRole = new[] { UserRoleEnum.ADMIN, UserRoleEnum.OWNER }
        },
        new ActionModel()
        {
            Action = ActionFilterEnum.RefreshToken,
            UserRole = new[] { UserRoleEnum.ADMIN, UserRoleEnum.OWNER, UserRoleEnum.ATTENDANT }
        },
        new ActionModel()
        {
            Action = ActionFilterEnum.UpdateProfileAccount,
            UserRole = new[] { UserRoleEnum.ADMIN, UserRoleEnum.OWNER }
        },
        new ActionModel()
        {
            Action = ActionFilterEnum.GetAccountProfile,
            UserRole = new[] { UserRoleEnum.ADMIN, UserRoleEnum.OWNER }
        }
    };

    public static ActionModel? GetAllActionModel(ActionFilterEnum action)
    {
        return AllActionModel.Find(x => x.Action == action);
    }
}