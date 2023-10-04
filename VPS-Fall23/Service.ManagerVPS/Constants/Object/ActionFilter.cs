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
        }
    };

    public static ActionModel? GetAllActionModel(ActionFilterEnum action)
    {
        return AllActionModel.Find(x => x.Action == action);
    }
}