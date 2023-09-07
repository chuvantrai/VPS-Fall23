using Service.ManagerCRM.Constants.Enums;
using Service.ManagerCRM.ModelsNoneDB.OtherModels;

namespace Service.ManagerCRM.Constants.Object;

public static class ActionFilter
{
    public static List<ActionModel> AllActionModel { get; } = new List<ActionModel>()
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