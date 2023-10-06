using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.DTO.Exceptions;

namespace Service.ManagerVPS.Attributes;

public class VpsActionFilterAttribute : ActionFilterAttribute
{
    public ActionFilterEnum Action { get; set; }

    public override void OnActionExecuting(ActionExecutingContext actionContext)
    {
        //Unauthorize
        throw new UnAuthorizeException();

        // Có lỗi permssion thì throw như sau
        throw new ForbidenException();


    }
}