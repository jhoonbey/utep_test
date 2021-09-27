using app.domain.Enums;
using app.domain.Model.Entities;
using app.Model.Criterias;
using app.web.core;
using System;
using System.Web.Mvc;

namespace app.web.admin.Controllers
{
    [User(AllowedRole = EnumUserRole.Admin)]
    public class TransportTypesController : BaseController
    {
        public TransportTypesController() : base("App_Utep")
        {
        }

        public ActionResult List(int pageNumber = 1)
        {
            int rowsPerPage = 10;
            try
            {
                var response = Service.LoadTransportTypesByCriteria(new TransportTypeCriteriaModel(), rowsPerPage, pageNumber, SessionInfo.Username, SessionInfo.Password);
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

        //public ActionResult View(int id)
        //{
        //    try
        //    {
        //        var result = Database.GetTransportTypeViewModel(id);
        //        return View(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["RedirectAlert"] = FillAlertModel(2, ex.Message);
        //        return RedirectToAction("List", "TransportTypes");
        //    }
        //}

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TransportType model)
        {
            try
            {
                var response = Service.CreateTransportType(model, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }

                return RedirectToAction("List", "TransportTypes");

            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                return View(model);
            }
        }


        public ActionResult Edit(int id)
        {
            try
            {
                var response = Service.GetTransportTypeById(id, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }

                return View(response.Model);
            }
            catch (Exception ex)
            {
                TempData["RedirectAlert"] = FillAlertModel(AlertStatus.Error, ex.Message);
                return RedirectToAction("List", "TransportTypes");
            }
        }

        [HttpPost]
        public ActionResult Edit(TransportType model)
        {
            try
            {
                var response = Service.EditTransportType(model, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }
                return RedirectToAction("List", "TransportTypes");
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var response = Service.DeleteTransportType(id, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }

                TempData["RedirectAlert"] = FillAlertModel(AlertStatus.Success, "Transport type deleted successfully");
                return RedirectToAction("List", "TransportTypes");
            }
            catch (Exception ex)
            {
                TempData["RedirectAlert"] = FillAlertModel(AlertStatus.Error, ex.Message);
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

    }
}
