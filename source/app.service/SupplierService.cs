using app.domain.Model.Data;
using app.domain.Model.Entities;
using app.Model.Criterias;
using app.domain.Model.EntityCollections;
using app.service.Models;
using System;
using System.Web;

namespace app.service
{
    public partial class UtepService
    {
        public BoolServiceResponse CreateSupplier(Supplier model, HttpPostedFileBase inter, HttpPostedFileBase lic, HttpPostedFileBase pass, HttpPostedFileBase pass2,
                                                                  HttpPostedFileBase front, HttpPostedFileBase back, HttpPostedFileBase right, HttpPostedFileBase left,
                                                 string username, string password)
        {
            var response = new BoolServiceResponse();
            try
            {
                response.Model = DataBase.CreateSupplier(model, inter, lic, pass, pass2, front, back, right, left, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public BoolServiceResponse EditSupplier(Supplier model, HttpPostedFileBase inter, HttpPostedFileBase lic, HttpPostedFileBase pass, HttpPostedFileBase pass2,
                                                                HttpPostedFileBase front, HttpPostedFileBase back, HttpPostedFileBase right, HttpPostedFileBase left,
                                                string username, string password)
        {
            var response = new BoolServiceResponse();
            try
            {
                response.Model = DataBase.EditSupplier(model, inter, lic, pass, pass2, front, back, right, left, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public BoolServiceResponse DeleteSupplier(int id, string username, string password)
        {
            var response = new BoolServiceResponse();
            try
            {
                response.Model = DataBase.DeleteSupplier(id, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public GenericServiceResponse<Supplier> GetSupplierById(int id, string username, string password)
        {
            var response = new GenericServiceResponse<Supplier>();
            try
            {
                response.Model = DataBase.GetSupplierById(id, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public GenericServiceResponse<SupplierDataModel> GetSupplierDataModel(int id, string username, string password)
        {
            var response = new GenericServiceResponse<SupplierDataModel>();
            try
            {
                response.Model = DataBase.GetSupplierDataModel(id, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }
        public GenericServiceResponse<SupplierEntityCollection> LoadSuppliersByCriteria(SupplierCriteriaModel criteria, int rowsPerPage, int pageNumber, string username, string password)
        {
            var response = new GenericServiceResponse<SupplierEntityCollection>();
            try
            {
                response.Model = DataBase.LoadSuppliersByCriteria(criteria, rowsPerPage, pageNumber, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }
    }
}