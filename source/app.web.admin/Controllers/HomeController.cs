using app.domain.Enums;
using app.service;
using app.web.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace app.web.admin.Controllers
{
    [User(AllowedRole = EnumUserRole.Admin)]
    public class HomeController : BaseController
    {
        public HomeController() : base("App_Utep")
        {

        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}