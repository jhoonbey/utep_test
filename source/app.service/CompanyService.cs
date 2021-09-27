using app.domain.Model.Entities;
using app.Model.Criterias;
using app.domain.Model.EntityCollections;
using app.service.Models;
using System;

namespace app.service
{
    public partial class UtepService
    {
         public BoolServiceResponse CreateCompany(Company model, string username, string password)
        {
            var response = new BoolServiceResponse();
            try
            {
                response.Model = DataBase.CreateCompany(model, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public BoolServiceResponse EditCompany(Company model, string username, string password)
        {
            var response = new BoolServiceResponse();
            try
            {
                response.Model = DataBase.EditCompany(model, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public BoolServiceResponse DeleteCompany(int id, string username, string password)
        {
            var response = new BoolServiceResponse();
            try
            {
                response.Model = DataBase.DeleteCompany(id, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public GenericServiceResponse<Company> GetCompanyById(int id, string username, string password)
        {
            var response = new GenericServiceResponse<Company>();
            try
            {
                response.Model = DataBase.GetCompanyById(id, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public GenericServiceResponse<CompanyEntityCollection> LoadCompaniesByCriteria(CompanyCriteriaModel criteria, int rowsPerPage, int pageNumber, string username, string password)
        {
            var response = new GenericServiceResponse<CompanyEntityCollection>();
            try
            {
                response.Model = DataBase.LoadCompaniesByCriteria(criteria, rowsPerPage, pageNumber, username, password);
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