using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Service.ManagerVPS.Constants.Enums;

namespace Service.ManagerVPS.FilterPermissions;

public class FilterPermission : ActionFilterAttribute
{
    public ActionFilterEnum Action { get; set; }

    public override void OnActionExecuting(ActionExecutingContext actionContext)
    {
        try
        {
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