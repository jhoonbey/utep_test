using app.domain.Enums;
using app.domain.Model.Entities;
using app.Model.Criterias;
using app.web.core;
using System;
using System.Web.Mvc;

namespace app.web.admin.Controllers
{
    [User(AllowedRole = EnumUserRole.Admin)]
    public class CompaniesController : BaseController
    {
        public CompaniesController() : base("App_Utep")
        {

        }

        public ActionResult List(int pageNumber = 1)
        {
            int rowsPerPage = 10;
            try
            {
                var response = Service.LoadCompaniesByCriteria(new CompanyCriteriaModel(), rowsPerPage, pageNumber, SessionInfo.Username, SessionInfo.Password);
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
        //        var result = Database.GetCompanyViewModel(id);
        //        return View(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["RedirectAlert"] = FillAlertModel(2, ex.Message);
        //        return RedirectToAction("List", "Companies");
        //    }
        //}

        public ActionResult Create()
        {
            var response = Service.LoadCountriesByCriteria(new CountryCriteriaModel(), 300, 1, SessionInfo.Username, SessionInfo.Password);
            if (!response.IsSuccessfull)
            {
                throw new Exception(response.ErrorMessage);
            }
            ViewBag.Countries = new SelectList(response.Model.Countries, "Id", "Name");


            var response2 = Service.LoadCitiesByCriteria(new CityCriteriaModel(), 10000, 1, SessionInfo.Username, SessionInfo.Password);
            if (!response2.IsSuccessfull)
            {
                throw new Exception(response2.ErrorMessage);
            }
            ViewBag.Cities = new SelectList(response2.Model.Cities, "Id", "Name");


            return View();
        }

        [HttpPost]
        public ActionResult Create(Company model)
        {
            try
            {
                var response = Service.CreateCompany(model, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }

                return RedirectToAction("List", "Companies");

            }
            catch (Exception ex)
            {
                AddError(ex.Message);


                var response = Service.LoadCountriesByCriteria(new CountryCriteriaModel(), 300, 1, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    AddError(response.ErrorMessage);
                }
                ViewBag.Countries = new SelectList(response.Model.Countries, "Id", "Name", model.CountryId);


                var response2 = Service.LoadCitiesByCriteria(new CityCriteriaModel { CountryId = model.CountryId }, 10000, 1, SessionInfo.Username, SessionInfo.Password);
                if (!response2.IsSuccessfull)
                {
                    throw new Exception(response2.ErrorMessage);
                }
                ViewBag.Cities = new SelectList(response2.Model.Cities, "Id", "Name", model.CityId);


                return View(model);
            }
        }


        public ActionResult Edit(int id)
        {
            try
            {
                var response = Service.GetCompanyById(id, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }


                //ddl
                var response2 = Service.LoadCountriesByCriteria(new CountryCriteriaModel { }, 300, 1, SessionInfo.Username, SessionInfo.Password);
                if (!response2.IsSuccessfull)
                {
                    throw new Exception(response2.ErrorMessage);
                }
                ViewBag.Countries = new SelectList(response2.Model.Countries, "Id", "Name", response.Model.CountryId);

                var response3 = Service.LoadCitiesByCriteria(new CityCriteriaModel { CountryId = response.Model.CountryId }, 10000, 1, SessionInfo.Username, SessionInfo.Password);
                if (!response3.IsSuccessfull)
                {
                    throw new Exception(response3.ErrorMessage);
                }
                ViewBag.Cities = new SelectList(response3.Model.Cities, "Id", "Name", response.Model.CityId);



                return View(response.Model);
            }
            catch (Exception ex)
            {
                TempData["RedirectAlert"] = FillAlertModel(AlertStatus.Error, ex.Message);
                return RedirectToAction("List", "Companies");
            }
        }

        [HttpPost]
        public ActionResult Edit(Company model)
        {
            try
            {
                var response = Service.EditCompany(model, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }
                return RedirectToAction("List", "Companies");
            }
            catch (Exception ex)
            {
                AddError(ex.Message);


                //ddl
                var response = Service.LoadCountriesByCriteria(new CountryCriteriaModel(), 300, 1, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    AddError(response.ErrorMessage);
                }
                ViewBag.Countries = new SelectList(response.Model.Countries, "Id", "Name", model.CountryId);

                var response2 = Service.LoadCitiesByCriteria(new CityCriteriaModel { CountryId = model.CountryId }, 10000, 1, SessionInfo.Username, SessionInfo.Password);
                if (!response2.IsSuccessfull)
                {
                    throw new Exception(response2.ErrorMessage);
                }
                ViewBag.Cities = new SelectList(response2.Model.Cities, "Id", "Name", model.CityId);


                return View(model);
            }
        }

        //[HttpPost]
        //public ActionResult UpdateImage(HttpPostedFileBase postedFile, int id)
        //{
        //    try
        //    {
        //        var result = Database.UpdateCompanyImage(postedFile, id, 259, 259, 262, 262, false);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["RedirectAlert"] = FillAlertModel(2, ex.Message);
        //    }

        //    return RedirectToAction("View", "Companies", new { id = id });
        //}

        public ActionResult Delete(int id)
        {
            try
            {
                var response = Service.DeleteCompany(id, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }

                TempData["RedirectAlert"] = FillAlertModel(AlertStatus.Success, "Company deleted successfully");

                return RedirectToAction("List", "Companies");
            }
            catch (Exception ex)
            {
                TempData["RedirectAlert"] = FillAlertModel(AlertStatus.Error, ex.Message);
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

    }
}
