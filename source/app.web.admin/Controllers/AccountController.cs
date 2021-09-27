using app.domain.Enums;
using app.domain.Model.Entities;
using app.Model.Criterias;
using app.web.admin.Models;
using app.web.core;
using System;
using System.DataManager;
using System.Linq;
using System.Web.Mvc;

namespace app.web.admin.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController() : base("App_Utep")
        {

        }

        [User(AllowedRole = EnumUserRole.SuperAdmin)]
        public ActionResult List(int pageNumber = 1)
        {
            int rowsPerPage = 10;
            try
            {
                var response = Service.LoadUsersByCriteria(new UserCriteriaModel(), rowsPerPage, pageNumber, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }
                ViewBag.PageNumber = pageNumber;
                ViewBag.RowsPerPage = rowsPerPage;
                ViewBag.NumberOfPages = GetPageNumber(response.Model.AllCount, rowsPerPage);

                return View(response.Model);
            }
            catch (Exception ex)
            {
                TempData["RedirectAlert"] = FillAlertModel(AlertStatus.Error, ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }


        [User(AllowedRole = EnumUserRole.SuperAdmin)]
        public ActionResult Create()
        {
            try
            {
                var extractList = new int[] { (int)EnumUserRole.SuperAdmin };

                ViewBag.Roles = new SelectList(Service.LoadAllUserRoleEnumsWithout(extractList.ToList(), SessionInfo.Username, SessionInfo.Password).Model, "Id", "Name");

                return View();
            }
            catch (Exception ex)
            {
                TempData["RedirectAlert"] = FillAlertModel(AlertStatus.Error, ex.Message);
                return RedirectToAction("List", "Account");
            }
        }

        [HttpPost]
        [User(AllowedRole = EnumUserRole.SuperAdmin)]
        public ActionResult Create(User model)
        {
            try
            {
                var response = Service.CreateUser(model, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }

                return RedirectToAction("List", "Account");
            }
            catch (Exception ex)
            {
                AddError(ex.Message);

                var extractList = new int[] { (int)EnumUserRole.SuperAdmin };
                ViewBag.Roles = new SelectList(Service.LoadAllUserRoleEnumsWithout(extractList.ToList(), SessionInfo.Username, SessionInfo.Password).Model, "Id", "Name", model.Role);

                return View(model);
            }
        }

        [User(AllowedRole = EnumUserRole.SuperAdmin)]
        public ActionResult Edit(int id)
        {
            try
            {
                var response = Service.GetUserById(id, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }

                var extractList = new int[] { (int)EnumUserRole.SuperAdmin };

                ViewBag.Roles = new SelectList(Service.LoadAllUserRoleEnumsWithout(extractList.ToList(), SessionInfo.Username, SessionInfo.Password).Model, "Id", "Name", response.Model.Role);

                return View(response.Model);
            }
            catch (Exception ex)
            {
                TempData["RedirectAlert"] = FillAlertModel(AlertStatus.Error, ex.Message);
                return RedirectToAction("List", "Account");
            }
        }

        [HttpPost]
        [User(AllowedRole = EnumUserRole.SuperAdmin)]
        public ActionResult Edit(User model)
        {
            try
            {
                var response = Service.EditUser(model, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }

                return RedirectToAction("List", "Account");
            }
            catch (Exception ex)
            {
                AddError(ex.Message);

                var extractList = new int[] { (int)EnumUserRole.SuperAdmin };
                ViewBag.Roles = new SelectList(Service.LoadAllUserRoleEnumsWithout(extractList.ToList(), SessionInfo.Username, SessionInfo.Password).Model, "Id", "Name", model.Role);

                return View(model);
            }
        }

        //[User(AllowedRole = EnumUserRole.SuperAdmin)]
        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        Database.DeleteUser(id);
        //        TempData["RedirectAlert"] = FillAlertModel(1, "User deleted successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["RedirectAlert"] = FillAlertModel(2, ex.Message);
        //    }
        //    return RedirectToAction("List", "Account", new { area = "Addmein" });
        //}


        [User(AllowedRole = EnumUserRole.SuperAdmin)]
        public ActionResult ResetPassword(int id)
        {
            try
            {
                var response = Service.ResetUserPassword(SessionInfo.Id, SessionInfo.Role, id, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }

                return this.Json(new { success = true, pass = response.Model, error = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return this.Json(new { success = false, pass = "", error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [User]
        public ActionResult ChangePassword()
        {
            return View();
        }


        [User]
        [HttpPost]
        public ActionResult ChangePassword(PasswordChangeModel model)
        {
            try
            {
                var response = Service.ChangeUserPassword(SessionInfo.Id, model.OldPassword, model.NewPassword, model.NewPasswordAgain, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull || string.IsNullOrEmpty(response.Model))
                {
                    throw new Exception(response.ErrorMessage);
                }

                TempData["RedirectAlert"] = FillAlertModel(AlertStatus.Success, "Your password changed successfully");

                //change password in cookie
                SessionInfo.ChangePassword(response.Model);

                return RedirectToAction("ChangePasswordResult");
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                return View(model);
            }
        }

        [User]
        public ActionResult ChangePasswordResult()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            try
            {
                var result = Service.GetUserByUsernameAndPassword(model.Username, model.Password);
                if (!result.IsSuccessfull || result.Model == null)
                {
                    throw new Exception(result.ErrorMessage);
                }
                else
                {
                    SessionInfo.IsAuthorized = true;
                    SessionInfo.Id = result.Model.Id;
                    SessionInfo.Fullname = result.Model.Fullname;
                    SessionInfo.Username = result.Model.Username;
                    SessionInfo.Password = Reverse.E2(result.Model.Password);
                    SessionInfo.Role = result.Model.Role;
                    SessionInfo.SaveValues();
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                return View(model);
            }
        }

        public ActionResult Logout()
        {
            if (SessionInfo.IsAuthorized)
                SessionInfo.ClearValues();
            return RedirectToAction("Login", "Account");
        }
    }
}
