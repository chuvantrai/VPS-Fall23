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
            UserRole = new[] { UserRoleEnum.ADMIN, UserRoleEnum.OWNER }
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
        },
        new ActionModel
        {
            Action = ActionFilterEnum.BlockUserAccount,
            UserRole = new[] { UserRoleEnum.ADMIN, UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.GetApprovedParkingZoneByOwnerId,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.GetAllParkingZoneOwner,
            UserRole = new[] { UserRoleEnum.ADMIN }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.GetListPromoCode,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.CreateNewPromoCode,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.GetPromoCodeDetail,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.UpdatePromoCode,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.GetReportForAdmin,
            UserRole = new[] { UserRoleEnum.ADMIN }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.GetTypeReport,
            UserRole = new[] { UserRoleEnum.ADMIN }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.FilterReport,
            UserRole = new[] { UserRoleEnum.ADMIN }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.DeleteParkingZoneAction,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.CancelAbsent,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.DeletePromoCode,
            UserRole = new[] { UserRoleEnum.OWNER }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.UpdateStatusReport,
            UserRole = new[] { UserRoleEnum.ADMIN }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.GetAddressListParkingZone,
            UserRole = new[] { UserRoleEnum.ADMIN }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.UpdateIsBlockAddress,
            UserRole = new[] { UserRoleEnum.ADMIN }
        },
        new ActionModel
        {
            Action = ActionFilterEnum.CreateAddress,
            UserRole = new[] { UserRoleEnum.ADMIN }
        }
    };

    public static ActionModel? GetAllActionModel(ActionFilterEnum action)
    {
        return AllActionModel.Find(x => x.Action == action);
    }
}