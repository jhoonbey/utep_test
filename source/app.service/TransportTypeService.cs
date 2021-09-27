using app.domain.Model.Entities;
using app.Model.Criterias;
using app.domain.Model.EntityCollections;
using app.service.Models;
using System;

namespace app.service
{
    public partial class UtepService
    {
         public BoolServiceResponse CreateTransportType(TransportType model, string username, string password)
        {
            var response = new BoolServiceResponse();
            try
            {
                response.Model = DataBase.CreateTransportType(model, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public BoolServiceResponse EditTransportType(TransportType model, string username, string password)
        {
            var response = new BoolServiceResponse();
            try
            {
                response.Model = DataBase.EditTransportType(model, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public BoolServiceResponse DeleteTransportType(int id, string username, string password)
        {
            var response = new BoolServiceResponse();
            try
            {
                response.Model = DataBase.DeleteTransportType(id, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public GenericServiceResponse<TransportType> GetTransportTypeById(int id, string username, string password)
        {
            var response = new GenericServiceResponse<TransportType>();
            try
            {
                response.Model = DataBase.GetTransportTypeById(id, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public GenericServiceResponse<TransportTypeEntityCollection> LoadTransportTypesByCriteria(TransportTypeCriteriaModel criteria, int rowsPerPage, int pageNumber, string username, string password)
        {
            var response = new GenericServiceResponse<TransportTypeEntityCollection>();
            try
            {
                response.Model = DataBase.LoadTransportTypesByCriteria(criteria, rowsPerPage, pageNumber, username, password);
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