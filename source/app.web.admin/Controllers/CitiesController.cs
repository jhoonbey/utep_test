using app.domain.Enums;
using app.domain.Model.Entities;
using app.Model.Criterias;
using app.web.core;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace app.web.admin.Controllers
{
    [User(AllowedRole = EnumUserRole.Admin)]
    public class CitiesController : BaseController
    {
        public CitiesController() : base("App_Utep") { }

        public ActionResult List(int pageNumber = 1)
        {
            int rowsPerPage = 10;
            try
            {
                var response = Service.LoadCitiesByCriteria(new CityCriteriaModel(), rowsPerPage, pageNumber, SessionInfo.Username, SessionInfo.Password);
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
        //        var result = Database.GetCityViewModel(id);
        //        return View(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["RedirectAlert"] = FillAlertModel(2, ex.Message);
        //        return RedirectToAction("List", "Cities");
        //    }
        //}

        public ActionResult LoadCitiesByCountryId(int parentId)
        {
            try
            {
                var response = Service.LoadCitiesByCriteria(new CityCriteriaModel { CountryId = parentId }, 1000, 1, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }

                return Json(response.Model.Cities, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                AddError(ex.Message);
                return Json(new List<City>(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Create()
        {
            var response = Service.LoadCountriesByCriteria(new CountryCriteriaModel(), 10000, 1, SessionInfo.Username, SessionInfo.Password);
            if (!response.IsSuccessfull)
            {
                throw new Exception(response.ErrorMessage);
            }

            ViewBag.Countries = new SelectList(response.Model.Countries, "Id", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Create(City model)
        {
            try
            {
                var response = Service.CreateCity(model, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }

                return RedirectToAction("List", "Cities");

            }
            catch (Exception ex)
            {
                AddError(ex.Message);

                var response = Service.LoadCountriesByCriteria(new CountryCriteriaModel(), 10000, 1, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    AddError(response.ErrorMessage);
                }
                ViewBag.Countries = new SelectList(response.Model.Countries, "Id", "Name", model.CountryId);

                return View(model);
            }
        }


        public ActionResult Edit(int id)
        {
            try
            {
                var response = Service.GetCityById(id, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }

                //ddl
                var response2 = Service.LoadCountriesByCriteria(new CountryCriteriaModel(), 10000, 1, SessionInfo.Username, SessionInfo.Password);
                if (!response2.IsSuccessfull)
                {
                    throw new Exception(response2.ErrorMessage);
                }
                ViewBag.Countries = new SelectList(response2.Model.Countries, "Id", "Name", response.Model.CountryId);


                return View(response.Model);
            }
            catch (Exception ex)
            {
                TempData["RedirectAlert"] = FillAlertModel(AlertStatus.Error, ex.Message);
                return RedirectToAction("List", "Cities");
            }
        }

        [HttpPost]
        public ActionResult Edit(City model)
        {
            try
            {
                var response = Service.EditCity(model, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }
                return RedirectToAction("List", "Cities");
                //return RedirectToAction("View", "Cities", new { id = model.Id });
            }
            catch (Exception ex)
            {
                AddError(ex.Message);

                //ddl
                var response = Service.LoadCountriesByCriteria(new CountryCriteriaModel(), 10000, 1, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    AddError(response.ErrorMessage);
                }
                ViewBag.Countries = new SelectList(response.Model.Countries, "Id", "Name", model.CountryId);


                return View(model);
            }
        }



        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        Database.DeleteCity(id);
        //        TempData["RedirectAlert"] = FillAlertModel(1, "City deleted successfully");
        //        return RedirectToAction("List", "Cities");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["RedirectAlert"] = FillAlertModel(2, ex.Message);
        //    }

        //    return Redirect(Request.UrlReferrer.ToString());
        //}
    }
}
