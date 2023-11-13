using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.DTO.OtherModels;

namespace Service.ManagerVPS.Constants.Object;

public static class ActionFilter
{
    private static List<ActionModel> AllActionModel { get; } = new()
    {
        new ActionModel
        {
            Action = ActionFilterEnum.GetOk,
            UserRole = null
        },
        new ActionModel
        {
            Action = ActionFilterEnum.AddUser,
            UserRole = new[] { UserRoleEnum.ADMIN }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.TestAuthApi,
            UserRole = new[] { UserRoleEnum.ADMIN }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.CreateAccountDemo,
            UserRole = null
        },
        new ActionModel
        {
            Action = ActionFilterEnum.ChangePassword,
            UserRole = new[] { UserRoleEnum.ADMIN, UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.RefreshToken,
            UserRole = new[] { UserRoleEnum.ADMIN, UserRoleEnum.OWNER, UserRoleEnum.ATTENDANT }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.UpdateProfileAccount,
            UserRole = new[] { UserRoleEnum.ADMIN, UserRoleEnum.OWNER }
        },
        new ActionModel()
        {
            Action = ActionFilterEnum.GetAccountProfile,
            UserRole = new[] { UserRoleEnum.ADMIN, UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.GetRequestedParkingZones,
            UserRole = new[] { UserRoleEnum.ADMIN }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.CreatApiBankingDemo,
            UserRole = null
        },
        new ActionModel
        {
            Action = ActionFilterEnum.CreateReport,
            UserRole = null
        },
        new ActionModel
        {
            Action = ActionFilterEnum.RegisterNewParkingZone,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.GetParkingZoneInfoById,
            UserRole = new[] { UserRoleEnum.OWNER, UserRoleEnum.ADMIN }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.ChangeParkingZoneStat,
            UserRole = new[] { UserRoleEnum.ADMIN }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.GetAllParkingZones,
            UserRole = new[] { UserRoleEnum.ADMIN, UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.ChangeParkingZoneFullStatus,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.UpdateParkingZone,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.CreateAttendantAccount,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.GetAllParkingZoneByOwnerId,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.GetListAttendant,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.SearchAttendantByName,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.CloseParkingZone,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.DeleteParkingZoneAndAbsent,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.GetFeedbackForOwner,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.AddReplyToFeedback,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.FilterFeedback,
            UserRole = new[] { UserRoleEnum.OWNER }
        }
    };

    public static ActionModel? GetAllActionModel(ActionFilterEnum action)
    {
        return AllActionModel.Find(x => x.Action == action);
    }
}