using app.domain.Model.Entities;
using app.service;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace app.web.core
{
    public class BaseController : Controller
    {
        public BaseController(string cookieName)
        {
            _cookieName = cookieName;

            Service = new UtepService();

            //ViewBag.SeoKeyword = ConfigurationManager.AppSettings["seokeyword"];
            //ViewBag.ContactData = Database.GetContactViewModel();

            ViewBag.FileRoot = ConfigurationManager.AppSettings["fileroot"];
        }

        private string _cookieName;
        public ClientAlertModel AlertModel { get; set; }
        public SessionInfo SessionInfo { get; set; }
        public IUtepService Service { get; set; }
        public User CurrentUser { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SessionInfo = new SessionInfo(this, _cookieName);

            ViewBag.SessionInfo = SessionInfo;
            ViewBag.CurrentUser = CurrentUser;

            base.OnActionExecuting(filterContext);
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            ModelState.AddModelError("", filterContext.Exception.Message);
        }

        public void AddError(string errorText)
        {
            ModelState.AddModelError("errorKey", errorText);
        }
        public void AddError(Exception exception)
        {
            AddError(exception.Message);
        }
        public RedirectToRouteResult RedirectOnError(Exception exception)
        {
            AddError(exception);
            return RedirectToAction("Error", "Home", new { area = "" });
        }

        public static int GetPageNumber(int allCount, int rowsPerPage)
        {
            if (rowsPerPage != 0)
            {
                var pageCount = allCount;
                if (pageCount % rowsPerPage == 0)
                    return pageCount / rowsPerPage;
                return (pageCount / rowsPerPage) + 1;
            }
            return allCount;
        }

        public ClientAlertModel FillAlertModel(AlertStatus status, string message)
        {
            return new ClientAlertModel(status, message);
        }
    }
}
