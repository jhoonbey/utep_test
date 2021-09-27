using app.domain.Model.Entities;
using app.Model.Criterias;
using app.domain.Model.EntityCollections;
using app.service.Models;
using System;

namespace app.service
{
    public partial class UtepService
    {
        public BoolServiceResponse CreateCountry(Country model, string username, string password)
        {
            var response = new BoolServiceResponse();
            try
            {
                response.Model = DataBase.CreateCountry(model, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public BoolServiceResponse EditCountry(Country model, string username, string password)
        {
            var response = new BoolServiceResponse();
            try
            {
                response.Model = DataBase.EditCountry(model, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }
        public GenericServiceResponse<Country> GetCountryById(int id, string username, string password)
        {
            var response = new GenericServiceResponse<Country>();
            try
            {
                response.Model = DataBase.GetCountryById(id, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public GenericServiceResponse<CountryEntityCollection> LoadCountriesByCriteria(CountryCriteriaModel criteria, int rowsPerPage, int pageNumber, string username, string password)
        {
            var response = new GenericServiceResponse<CountryEntityCollection>();
            try
            {
                response.Model = DataBase.LoadCountriesByCriteria(criteria, rowsPerPage, pageNumber, username, password);
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