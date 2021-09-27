using app.domain.Enums;
using System.Web.Mvc;
using System.Web.Routing;

namespace app.web.core
{
    public class UserAttribute : ActionFilterAttribute
    {
        public EnumUserRole AllowedRole { get; set; }

        private bool IsLoginPage(ActionExecutingContext filterContext)
        {
            var areaValue = filterContext.RouteData.Values["Area"];
            var area = areaValue != null ? areaValue.ToString() : string.Empty;
            var controller = filterContext.RouteData.Values["Controller"].ToString();
            var action = filterContext.RouteData.Values["Action"].ToString();
            return area == string.Empty && controller == "Account" && action == "Login";
        }

        private bool IsAuthorizedUser(BaseController controller)
        {
            if (controller.SessionInfo.IsAuthorized)
            {
                var result = controller.Service.GetUserByIdentification(controller.SessionInfo.Username, controller.SessionInfo.Password, controller.SessionInfo.Role);
                if (result.IsSuccessfull && result.Model != null)
                {
                    controller.CurrentUser = result.Model;
                    return true;
                }
            }
            return false;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            BaseController controller = filterContext.Controller as BaseController;
            if (controller != null)
            {
                bool isLoginPage = IsLoginPage(filterContext);
                bool isAuthorizedUser = IsAuthorizedUser(controller);

                if (!isAuthorizedUser || !controller.SessionInfo.IsAuthorized || controller.CurrentUser == null)
                {
                    controller.SessionInfo.ClearValues();
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Login", controller = "Account", area = "" }));
                    return;
                }

                if (controller.CurrentUser.Role == (int)EnumUserRole.Admin)
                {
                    // it is ok, user can pass
                }
                else
                {
                    if (controller.CurrentUser.Role < (int)AllowedRole)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Login", controller = "Account", area = "" }));
                        return;
                    }
                }
            }
        }
    }
}
