using app.domain.Enums;
using app.domain.Model.Entities;
using app.Model.Criterias;
using app.web.core;
using System;
using System.Web;
using System.Web.Mvc;

namespace app.web.admin.Controllers
{
    [User(AllowedRole = EnumUserRole.Admin)]
    public class SuppliersController : BaseController
    {
        public SuppliersController() : base("App_Utep")
        {
        }

        public ActionResult List(int pageNumber = 1)
        {
            int rowsPerPage = 10;
            try
            {
                var response = Service.LoadSuppliersByCriteria(new SupplierCriteriaModel(), rowsPerPage, pageNumber, SessionInfo.Username, SessionInfo.Password);
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

        public ActionResult Details(int id)
        {
            try
            {
                var response = Service.GetSupplierDataModel(id, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }
                return View(response.Model);
            }
            catch (Exception ex)
            {
                TempData["RedirectAlert"] = FillAlertModel(AlertStatus.Error, ex.Message);
                return RedirectToAction("List", "Suppliers");
            }
        }

        public ActionResult Create()
        {
            var response = Service.LoadTransportTypesByCriteria(new TransportTypeCriteriaModel(), 300, 1, SessionInfo.Username, SessionInfo.Password);
            if (!response.IsSuccessfull)
            {
                throw new Exception(response.ErrorMessage);
            }
            ViewBag.TransportTypes = new SelectList(response.Model.TransportTypes, "Id", "Name");

            var response2 = Service.LoadCompaniesByCriteria(new CompanyCriteriaModel(), 500, 1, SessionInfo.Username, SessionInfo.Password);
            if (!response2.IsSuccessfull)
            {
                throw new Exception(response2.ErrorMessage);
            }
            ViewBag.Companies = new SelectList(response2.Model.Companies, "Id", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Create(Supplier model, HttpPostedFileBase inter, HttpPostedFileBase lic, HttpPostedFileBase pass, HttpPostedFileBase pass2,
                                                   HttpPostedFileBase front, HttpPostedFileBase back, HttpPostedFileBase right, HttpPostedFileBase left)
        {
            try
            {
                var response = Service.CreateSupplier(model, front, back, right, left, pass, pass2, inter, lic, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }

                return RedirectToAction("List", "Suppliers");

            }
            catch (Exception ex)
            {
                AddError(ex.Message);

                var response = Service.LoadTransportTypesByCriteria(new TransportTypeCriteriaModel(), 300, 1, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }
                ViewBag.TransportTypes = new SelectList(response.Model.TransportTypes, "Id", "Name", model.TransportTypeId);

                var response2 = Service.LoadCompaniesByCriteria(new CompanyCriteriaModel(), 500, 1, SessionInfo.Username, SessionInfo.Password);
                if (!response2.IsSuccessfull)
                {
                    throw new Exception(response2.ErrorMessage);
                }
                ViewBag.Companies = new SelectList(response2.Model.Companies, "Id", "Name", model.CompanyId);

                return View(model);
            }
        }


        public ActionResult Edit(int id)
        {
            try
            {
                var response = Service.GetSupplierById(id, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }


                //ddl
                var response1 = Service.LoadTransportTypesByCriteria(new TransportTypeCriteriaModel(), 300, 1, SessionInfo.Username, SessionInfo.Password);
                if (!response1.IsSuccessfull)
                {
                    throw new Exception(response1.ErrorMessage);
                }
                ViewBag.TransportTypes = new SelectList(response1.Model.TransportTypes, "Id", "Name", response.Model.TransportTypeId);

                var response2 = Service.LoadCompaniesByCriteria(new CompanyCriteriaModel(), 500, 1, SessionInfo.Username, SessionInfo.Password);
                if (!response2.IsSuccessfull)
                {
                    throw new Exception(response2.ErrorMessage);
                }
                ViewBag.Companies = new SelectList(response2.Model.Companies, "Id", "Name", response.Model.CompanyId);


                return View(response.Model);
            }
            catch (Exception ex)
            {
                TempData["RedirectAlert"] = FillAlertModel(AlertStatus.Error, ex.Message);
                return RedirectToAction("List", "Suppliers");
            }
        }

        [HttpPost]
        public ActionResult Edit(Supplier model, HttpPostedFileBase inter, HttpPostedFileBase lic, HttpPostedFileBase pass, HttpPostedFileBase pass2,
                                                   HttpPostedFileBase front, HttpPostedFileBase back, HttpPostedFileBase right, HttpPostedFileBase left)
        {
            try
            {
                var response = Service.EditSupplier(model, front, back, right, left, pass, pass2, inter, lic, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }
                return RedirectToAction("List", "Suppliers");
            }
            catch (Exception ex)
            {
                AddError(ex.Message);

                //ddl
                var response1 = Service.LoadTransportTypesByCriteria(new TransportTypeCriteriaModel(), 300, 1, SessionInfo.Username, SessionInfo.Password);
                if (!response1.IsSuccessfull)
                {
                    throw new Exception(response1.ErrorMessage);
                }
                ViewBag.TransportTypes = new SelectList(response1.Model.TransportTypes, "Id", "Name", model.TransportTypeId);

                var response2 = Service.LoadCompaniesByCriteria(new CompanyCriteriaModel(), 500, 1, SessionInfo.Username, SessionInfo.Password);
                if (!response2.IsSuccessfull)
                {
                    throw new Exception(response2.ErrorMessage);
                }
                ViewBag.Companies = new SelectList(response2.Model.Companies, "Id", "Name", model.CompanyId);

                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var response = Service.DeleteSupplier(id, SessionInfo.Username, SessionInfo.Password);
                if (!response.IsSuccessfull)
                {
                    throw new Exception(response.ErrorMessage);
                }

                TempData["RedirectAlert"] = FillAlertModel(AlertStatus.Success, "Supplier deleted successfully");
                return RedirectToAction("List", "Suppliers");
            }
            catch (Exception ex)
            {
                TempData["RedirectAlert"] = FillAlertModel(AlertStatus.Error, ex.Message);
            }

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}
