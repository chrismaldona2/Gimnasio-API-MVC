using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace WebApp.ViewPermissions
{
    public class ValidarSesionAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sesion = context.HttpContext.Session;

            if (sesion.GetString("AdminLogueado") == null)
            {
                context.Result = new RedirectResult("~/Usuario/LoginAdmin");
            }
            base.OnActionExecuting(context);
        }

    }
}
