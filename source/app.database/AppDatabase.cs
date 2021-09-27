using app.database.Validations;
using System;
using System.Configuration;
using System.Data.SqlClient;

namespace app.database
{
    public partial class AppDatabase
    {
        public AppDatabase()
        {
            var conn_str = ConfigurationManager.AppSettings["connectionString"];
            _conn = new SqlConnection(conn_str);
        }

        private readonly SqlConnection _conn;
        public void CloseConnection()
        {
            if (_conn.State != System.Data.ConnectionState.Closed)
            {
                _conn.Close();
            }
        }
        public void OpenConnection()
        {
            if (_conn.State != System.Data.ConnectionState.Open)
            {
                _conn.Open();
            }
        }

        //public HomeViewModel GetHomeViewModel()
        //{
        //    HomeViewModel result = new HomeViewModel();

        //    try
        //    {
        //        OpenConnection();

        //        //result.Images = loadImagesByCriteria(new ImageCriteriaModel { Sector = "HomeSlide" }, 100, 1);
        //        result.Categories = loadCategories();
        //        //result.Branches = loadBranches();
        //        result.Products = loadProductsByCriteria(new ProductCriteriaModel(), 20, 1);
        //        result.Services = loadServicesByCriteria(new ServiceCriteriaModel(), 30, 1);
        //        result.Colors = loadColorsByCriteria(new ColorCriteriaModel(), 10, 1);
        //        result.TermsAndConditions = getOptionBySec("TermAndCondition").Val;
        //        result.About = getOptionBySec("About").Val;

        //        //result.Newss = loadNewssByCriteria(new NewsCriteriaModel(), 2, 1);
        //        //result.Clients = loadClientsByCriteria(new ClientCriteriaModel(), 10, 1);
        //        //result.Brands = loadBrandsByCriteria(new BrandCriteriaModel { Show = true }, 1000, 1);

        //        CloseConnection();
        //    }
        //    catch (Exception)
        //    {
        //        CloseConnection();
        //        throw;
        //    }

        //    return result;
        //}

        public object CheckForDbNull(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return value;
        }
        public DateTime GetDateTimeNow()
        {
            int min_Diff = 0;
            try
            {
                int diff = Convert.ToInt32(ConfigurationManager.AppSettings["min"].ToString());
                min_Diff = diff;
            }
            catch (Exception)
            {
            }
            return DateTime.Now.AddMinutes(min_Diff);
        }

        //public bool SendEmail(ContactEmailModel model)
        //{
        //    Validator.ContactEmailSendValidation(model);
        //    try
        //    {
        //        //from config
        //        string host = ConfigurationManager.AppSettings["emailHost"];
        //        int port = Convert.ToInt32(ConfigurationManager.AppSettings["emailPort"]);
        //        int timeout = Convert.ToInt32(ConfigurationManager.AppSettings["emailTimeOut"]);
        //        bool enableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["emailSsl"]);
        //        string username = ConfigurationManager.AppSettings["emailUsername"];
        //        string password = ConfigurationManager.AppSettings["emailPassword"];
        //        string emailFrom = ConfigurationManager.AppSettings["emailFrom"];
        //        string emailTo = ConfigurationManager.AppSettings["emailTo"]; 
        //        string subject = "E-mail which sent from " + ConfigurationManager.AppSettings["siteNameAdjacent"] + " site";

        //        string body = "Sender: " + model.Name + "  [ " + model.Email + " ]  " + string.Format(" <br /> <br />") + model.Message;
        //        return EHelper.Send(host, port, timeout, username, password, enableSsl, emailFrom, emailTo, subject, body);
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return false;
        //}

        //public ContactViewModel GetContactViewModel()
        //{
        //    try
        //    {
        //        OpenConnection();

        //        ContactViewModel model = new ContactViewModel();
        //        model.Address = getOptionBySec("ContactAddress").Val;
        //        model.Phone = getOptionBySec("ContactPhone").Val;
        //        model.Email = getOptionBySec("ContactMail").Val;

        //        CloseConnection();
        //        return model;
        //    }
        //    catch (Exception)
        //    {
        //        CloseConnection();
        //        throw;
        //    }
        //}

    }
}
