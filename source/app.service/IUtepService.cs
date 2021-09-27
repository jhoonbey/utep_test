using app.domain.Enums;
using app.domain.Model.Data;
using app.domain.Model.Entities;
using app.Model.Criterias;
using app.domain.Model.EntityCollections;
using app.service.Models;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web;

namespace app.service
{
    [ServiceContract]
    public interface IUtepService
    {
        #region Supplier
        [OperationContract]
        BoolServiceResponse CreateSupplier(Supplier model, HttpPostedFileBase inter, HttpPostedFileBase lic, HttpPostedFileBase pass, HttpPostedFileBase pass2, 
                                                           HttpPostedFileBase front, HttpPostedFileBase back, HttpPostedFileBase right, HttpPostedFileBase left,
                                           string username, string password);

        [OperationContract]
        BoolServiceResponse EditSupplier(Supplier model, HttpPostedFileBase inter, HttpPostedFileBase lic, HttpPostedFileBase pass, HttpPostedFileBase pass2,
                                                         HttpPostedFileBase front, HttpPostedFileBase back, HttpPostedFileBase right, HttpPostedFileBase left,
                                         string username, string password);

        [OperationContract]
        BoolServiceResponse DeleteSupplier(int id, string username, string password);

        [OperationContract]
        GenericServiceResponse<Supplier> GetSupplierById(int id, string username, string password);

        [OperationContract]
        GenericServiceResponse<SupplierDataModel> GetSupplierDataModel(int id, string username, string password);

        [OperationContract]
        GenericServiceResponse<SupplierEntityCollection> LoadSuppliersByCriteria(SupplierCriteriaModel criteria, int rowsPerPage, int pageNumber, string username, string password);
        #endregion


        #region User
        [OperationContract]
        BoolServiceResponse CreateUser(User model, string username, string password);

        [OperationContract]
        BoolServiceResponse EditUser(User model, string username, string password);

        [OperationContract]
        GenericServiceResponse<User> GetUserById(int id, string username, string password);

        [OperationContract]
        GenericServiceResponse<User> GetUserByUsernameAndPassword(string username, string password);

        [OperationContract]
        GenericServiceResponse<User> GetUserByIdentification(string username, string password, int role);

        [OperationContract]
        GenericServiceResponse<string> ChangeUserPassword(int loggedUserId, string oldPass, string newPass, string newPass2, string username, string password);

        [OperationContract]
        GenericServiceResponse<string> ResetUserPassword(int loggedUserId, int loggedUserRole, int userId, string username, string password);

        [OperationContract]
        GenericServiceResponse<UserEntityCollection> LoadUsersByCriteria(UserCriteriaModel criteria, int rowsPerPage, int pageNumber, string username, string password);

        [OperationContract]
        GenericServiceResponse<List<EnumModel>> LoadAllUserRoleEnumsWithout(List<int> idList, string username, string password);
        #endregion


        #region TransportType
        [OperationContract]
        BoolServiceResponse CreateTransportType(TransportType model, string username, string password);

        [OperationContract]
        BoolServiceResponse EditTransportType(TransportType model, string username, string password);
        [OperationContract]
        BoolServiceResponse DeleteTransportType(int id, string username, string password);
        [OperationContract]
        GenericServiceResponse<TransportType> GetTransportTypeById(int id, string username, string password);

        [OperationContract]
        GenericServiceResponse<TransportTypeEntityCollection> LoadTransportTypesByCriteria(TransportTypeCriteriaModel criteria, int rowsPerPage, int pageNumber, string username, string password);
        #endregion


        #region Company
        [OperationContract]
        BoolServiceResponse CreateCompany(Company model, string username, string password);

        [OperationContract]
        BoolServiceResponse EditCompany(Company model, string username, string password);

        [OperationContract]
        BoolServiceResponse DeleteCompany(int id, string username, string password);

        [OperationContract]
        GenericServiceResponse<Company> GetCompanyById(int id, string username, string password);

        [OperationContract]
        GenericServiceResponse<CompanyEntityCollection> LoadCompaniesByCriteria(CompanyCriteriaModel criteria, int rowsPerPage, int pageNumber, string username, string password);
        #endregion


        #region Country
        [OperationContract]
        BoolServiceResponse CreateCountry(Country model, string username, string password);

        [OperationContract]
        BoolServiceResponse EditCountry(Country model, string username, string password);

        [OperationContract]
        GenericServiceResponse<Country> GetCountryById(int id, string username, string password);

        [OperationContract]
        GenericServiceResponse<CountryEntityCollection> LoadCountriesByCriteria(CountryCriteriaModel criteria, int rowsPerPage, int pageNumber, string username, string password);
        #endregion

        #region City
        [OperationContract]
        BoolServiceResponse CreateCity(City model, string username, string password);

        [OperationContract]
        BoolServiceResponse EditCity(City model, string username, string password);

        [OperationContract]
        GenericServiceResponse<City> GetCityById(int id, string username, string password);

        [OperationContract]
        GenericServiceResponse<CityEntityCollection> LoadCitiesByCriteria(CityCriteriaModel criteria, int rowsPerPage, int pageNumber, string username, string password);
        #endregion
    }
}
