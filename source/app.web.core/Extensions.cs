using System.Web.Mvc;

namespace app.web.core
{
    public static class Extensions
    {
        public static SessionInfo GetSessionInfo(this WebViewPage parmThis)
        {
            var controller = parmThis.ViewContext.Controller as BaseController;
            if (controller != null)
            {
                return controller.SessionInfo;
            }
            return null;
        }
    }
}
