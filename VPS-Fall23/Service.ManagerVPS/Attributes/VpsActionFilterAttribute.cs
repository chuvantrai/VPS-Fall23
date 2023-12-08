using Microsoft.AspNetCore.Mvc.Filters;
using Service.ManagerVPS.Constants.Enums;

namespace Service.ManagerVPS.Attributes;

public class VpsActionFilterAttribute : ActionFilterAttribute
{
    public ActionFilterEnum Action { get; set; }

    public override void OnActionExecuting(ActionExecutingContext actionContext)
    {
        //Unauthorize
       // throw new UnauthorizeException();

        // Có lỗi permssion thì throw như sau
        //throw new ForbidenException();
    }
}