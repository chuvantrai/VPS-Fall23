using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.Object;
using Service.ManagerVPS.Extensions.StaticLogic;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.FilterPermissions;

public class FilterPermission : ActionFilterAttribute
{
    public ActionFilterEnum Action { get; set; }

    public override void OnActionExecuting(ActionExecutingContext actionContext)
    {
        try
        {
            var action = ActionFilter.GetAllActionModel(Action);
            if (action is { UserRole.Length: > 0 })
            {
                var userRoles = action.UserRole!;
                var httpContext = actionContext.HttpContext;
                var accessToken = httpContext.Request.Cookies["ACCESS_TOKEN"];
                if (!string.IsNullOrEmpty(accessToken))
                {
                    var userToken = JwtTokenExtension.ReadToken(accessToken);
                    if (userToken != null && DateTime.Now < userToken.Expires)
                    {
                        // get account in db
                        using var context = new FALL23_SWP490_G14Context();
                        var account =
                            context.Accounts.FirstOrDefault(x => x.Id.ToString().Equals(userToken.UserId.ToUpper()));
                        if (account != null &&
                            GeneralExtension.CheckEqualDateTime(account.ModifiedAt, userToken.ModifiedAt))
                        {
                            // check role Permission
                            if (userRoles.Contains(EnumExtension.CoverIntToEnum<UserRoleEnum>(userToken.RoleId)))
                            {
                                // Permission to access
                                return;
                            }
                        }
                    }
                }
            }
            else
            {
                // api publish
                return;
            }

            // No Permission
            actionContext.Result = new JsonResult(new
            {
                success = false,
                status = 401,
                error = "401 Unauthorized",
                hhh = actionContext.HttpContext.Request.Cookies["ACCESS_TOKEN"]
            });
        }
        catch
        {
            actionContext.Result = new JsonResult(new
            {
                success = false,
                status = 401,
                error = "401 Unauthorized"
            });
        }
    }
}