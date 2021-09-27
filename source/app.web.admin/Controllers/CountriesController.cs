using app.domain.Enums;
using app.domain.Model.Entities;
using app.Model.Criterias;
using app.web.core;
using System;
using System.Web.Mvc;

namespace app.web.admin.Controllers
{
    [User(AllowedRole = EnumUserRole.Admin)]
    public class CountriesController : BaseController
    {
        public CountriesController() : base("App_Utep") { }

        public ActionResult List(int pageNumber = 1)
        {
            int rowsPerPage = 10;
            try
            {
                var response = Service.LoadCountriesByCriteria(new CountryCriteriaModel(), rowsPerPage, pageNumber, SessionInfo.Username, SessionInfo.Password);
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
        //        var result = Database.GetCountryViewModel(id);
        //        return View(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["RedirectAlert"] = FillAlertModel(2, ex.Message);
        //        return RedirectToAction("List", "Countries");
        //    }
        //}

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Country model)
        {
            try
            {
                var response = Service.CreateCountry(model, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }

                return RedirectToAction("List", "Countries");

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
                var response = Service.GetCountryById(id, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }

                return View(response.Model);
            }
            catch (Exception ex)
            {
                TempData["RedirectAlert"] = FillAlertModel(AlertStatus.Error, ex.Message);
                return RedirectToAction("List", "Countries");
            }
        }

        [HttpPost]
        public ActionResult Edit(Country model)
        {
            try
            {
                var response = Service.EditCountry(model, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }
                return RedirectToAction("List", "Countries");
                //return RedirectToAction("View", "Countries", new { id = model.Id });
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                return View(model);
            }
        }



        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        Database.DeleteCountry(id);
        //        TempData["RedirectAlert"] = FillAlertModel(1, "Country deleted successfully");
        //        return RedirectToAction("List", "Countries");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["RedirectAlert"] = FillAlertModel(2, ex.Message);
        //    }

        //    return Redirect(Request.UrlReferrer.ToString());
        //}
    }
}
