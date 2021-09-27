using app.domain.Enums;
using app.domain.Model.Entities;
using app.Model.Criterias;
using app.domain.Model.EntityCollections;
using app.service.Models;
using System;
using System.Collections.Generic;

namespace app.service
{
    public partial class UtepService
    {
        public BoolServiceResponse CreateUser(User model, string username, string password)
        {
            var response = new BoolServiceResponse();
            try
            {
                response.Model = DataBase.CreateUser(model, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public BoolServiceResponse EditUser(User model, string username, string password)
        {
            var response = new BoolServiceResponse();
            try
            {
                response.Model = DataBase.EditUser(model, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public GenericServiceResponse<User> GetUserById(int id, string username, string password)
        {
            var response = new GenericServiceResponse<User>();
            try
            {
                response.Model = DataBase.GetUserById(id, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public GenericServiceResponse<User> GetUserByUsernameAndPassword(string username, string password)
        {
            var response = new GenericServiceResponse<User>();
            try
            {
                response.Model = DataBase.GetUserByUsernameAndPassword(username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }
        public GenericServiceResponse<User> GetUserByIdentification(string username, string password, int role)
        {
            var response = new GenericServiceResponse<User>();
            try
            {
                response.Model = DataBase.GetUserByIdentification(username, password, role);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public GenericServiceResponse<string> ChangeUserPassword(int loggedUserId, string oldPass, string newPass, string newPass2, string username, string password)
        {
            var response = new GenericServiceResponse<string>();
            try
            {
                response.Model = DataBase.ChangeUserPassword(loggedUserId, oldPass, newPass, newPass2, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public GenericServiceResponse<string> ResetUserPassword(int loggedUserId, int loggedUserRole, int userId, string username, string password)
        {
            var response = new GenericServiceResponse<string>();
            try
            {
                response.Model = DataBase.ResetUserPassword(loggedUserId, loggedUserRole, userId, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public GenericServiceResponse<UserEntityCollection> LoadUsersByCriteria(UserCriteriaModel criteria, int rowsPerPage, int pageNumber, string username, string password)
        {
            var response = new GenericServiceResponse<UserEntityCollection>();
            try
            {
                response.Model = DataBase.LoadUsersByCriteria(criteria, rowsPerPage, pageNumber, username, password);
                response.IsSuccessfull = true;
            }
            catch (Exception exp)
            {
                response.ErrorMessage = (exp.Message);
                response.IsSuccessfull = false;
            }
            return response;
        }

        public GenericServiceResponse<List<EnumModel>> LoadAllUserRoleEnumsWithout(List<int> idList, string username, string password)
        {
            var response = new GenericServiceResponse<List<EnumModel>>();
            try
            {
                response.Model = DataBase.LoadAllUserRoleEnumsWithout(idList, username, password);
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