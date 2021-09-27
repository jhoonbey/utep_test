using app.database.Validations;
using app.domain.Enums;
using app.domain.Model.Entities;
using app.Model.Criterias;
using app.domain.Model.EntityCollections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DataManager;
using System.Transactions;

namespace app.database
{
    public partial class AppDatabase
    {
        public bool CreateCompany(Company model, string username, string password)
        {
            ValidateAuthorization(username, password);
            Validator.CompanyCreateValidation(model);

            try
            {
                OpenConnection();

                //db validations
                if (checkCompanyExistByCompanyName(model.Name)) throw new Exception("This Name is busy");

                const string sql = "INSERT INTO Companies (Name,  Address,  Voen,  Mail,  Phone,  CountryId,  CityId,  IsDeleted,  CreateDate ) VALUES" +
                                                        "(@Name, @Address, @Voen, @Mail, @Phone, @CountryId, @CityId, @IsDeleted, @CreateDate)";

                using (var cmd = new SqlCommand(sql, _conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", model.Name);
                    cmd.Parameters.AddWithValue("@Address", CheckForDbNull(model.Address));
                    cmd.Parameters.AddWithValue("@Voen", CheckForDbNull(model.Voen));
                    cmd.Parameters.AddWithValue("@Mail", CheckForDbNull(model.Mail));
                    cmd.Parameters.AddWithValue("@Phone", CheckForDbNull(model.Phone));

                    cmd.Parameters.AddWithValue("@CountryId", model.CountryId);
                    cmd.Parameters.AddWithValue("@CityId", model.CityId);

                    cmd.Parameters.AddWithValue("@IsDeleted", false);
                    cmd.Parameters.AddWithValue("@CreateDate", GetDateTimeNow());
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }


        public bool EditCompany(Company model, string username, string password)
        {
            ValidateAuthorization(username, password);
            Validator.CompanyEditValidation(model);

            try
            {
                OpenConnection();

                //db validations
                if (checkCompanyExistByCompanyNameNotThis(model.Name, model.Id)) throw new Exception("This Name is busy");

                const string sql = "UPDATE Companies SET Name = @Name,  Address = @Address,  Voen = @Voen,  Mail = @Mail,  Phone = @Phone,  " +
                                                        "CountryId = @CountryId,  CityId = @CityId  WHERE Id = @Id AND IsDeleted = @IsDeleted";

                using (var cmd = new SqlCommand(sql, _conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Name", model.Name);
                    cmd.Parameters.AddWithValue("@Address", CheckForDbNull(model.Address));
                    cmd.Parameters.AddWithValue("@Voen", CheckForDbNull(model.Voen));
                    cmd.Parameters.AddWithValue("@Mail", CheckForDbNull(model.Mail));
                    cmd.Parameters.AddWithValue("@Phone", CheckForDbNull(model.Phone));
                    cmd.Parameters.AddWithValue("@CountryId", model.CountryId);
                    cmd.Parameters.AddWithValue("@CityId", model.CityId);

                    cmd.Parameters.AddWithValue("@IsDeleted", false);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool DeleteCompany(int id, string username, string password)
        {
            ValidateAuthorization(username, password);

            try
            {
                OpenConnection();

                var model = getCompanyById(id);
                if (model == null) throw new Exception("Company not found");

                if (checkSupplierExistBy("CompanyId", model.Id)) throw new Exception("This Company is related a Supplier. Firstly, delete it.");

                const string sql = "UPDATE Companies SET IsDeleted = @IsDeleted WHERE Id = @Id";
                using (var cmd = new SqlCommand(sql, _conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@IsDeleted", true);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }


        //get methods
        public Company GetCompanyById(int id, string username, string password)
        {
            ValidateAuthorization(username, password);

            try
            {
                OpenConnection();

                return getCompanyById(id);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        //public CompanyViewModel GetCompanyViewModel(int categoryId, string username, string password)
        //{
        //    try
        //    {
        //        OpenConnection();
        //        CompanyViewModel model = new CompanyViewModel();
        //        model.Company = getCompanyById(categoryId);

        //        if (model.Company == null)
        //            throw new Exception("Company not found");

        //        model.Products = loadProductsByCriteria(new ProductCriteriaModel { CompanyId = categoryId }, 10000, 1);
        //        CloseConnection();
        //        return model;
        //    }
        //    catch (Exception)
        //    {
        //        CloseConnection();
        //        throw;
        //    }
        //}


        public CompanyEntityCollection LoadCompaniesByCriteria(CompanyCriteriaModel criteria, int rowsPerPage, int pageNumber, string username, string password)
        {
            ValidateAuthorization(username, password);

            try
            {
                OpenConnection();

                CompanyEntityCollection result = new CompanyEntityCollection();
                result.Companies = new List<Company>();

                string _minCreateDate = (criteria.MinCreateDate == null || criteria.MinCreateDate.Value == DateTime.MinValue) ? string.Empty : " AND CreateDate >= @MinCreateDate ";
                string _maxCreateDate = (criteria.MaxCreateDate == null || criteria.MaxCreateDate.Value == DateTime.MinValue) ? string.Empty : " AND CreateDate <= @MaxCreateDate ";

                //calculate count
                string sqlCount = "SELECT COUNT(*) FROM Companies WHERE IsDeleted = @IsDeleted " + _minCreateDate + _maxCreateDate;
                using (var cmdCount = new SqlCommand(sqlCount, _conn))
                {
                    cmdCount.CommandType = CommandType.Text;
                    if (!string.IsNullOrEmpty(_minCreateDate)) { cmdCount.Parameters.AddWithValue("@MinCreateDate", criteria.MinCreateDate); }
                    if (!string.IsNullOrEmpty(_maxCreateDate)) { cmdCount.Parameters.AddWithValue("@MaxCreateDate", criteria.MaxCreateDate); }
                    cmdCount.Parameters.AddWithValue("@IsDeleted", false);
                    result.AllCount = Convert.ToInt32(cmdCount.ExecuteScalar());
                }

                //calculate list
                string sql = "SELECT Id, Name,  Address,  Voen,  Mail,  Phone,  CountryId,  CityId,  IsDeleted,  CreateDate FROM ( " +
                             "SELECT Id, Name,  Address,  Voen,  Mail,  Phone,  CountryId,  CityId,  IsDeleted,  CreateDate, " +
                             "ROW_NUMBER() OVER (ORDER BY CreateDate DESC) AS RowNum FROM Companies WHERE IsDeleted = @IsDeleted " +
                             _minCreateDate + _maxCreateDate +
                             " ) AS SOD WHERE SOD.RowNum BETWEEN @RowStart AND @RowEnd ORDER BY CreateDate DESC";
                using (var cmd = new SqlCommand(sql, _conn))
                {
                    cmd.CommandType = CommandType.Text;
                    if (!string.IsNullOrEmpty(_minCreateDate)) { cmd.Parameters.AddWithValue("@MinCreateDate", criteria.MinCreateDate); }
                    if (!string.IsNullOrEmpty(_maxCreateDate)) { cmd.Parameters.AddWithValue("@MaxCreateDate", criteria.MaxCreateDate); }
                    cmd.Parameters.AddWithValue("@RowStart", ((pageNumber - 1) * rowsPerPage) + 1);
                    cmd.Parameters.AddWithValue("@RowEnd", rowsPerPage * pageNumber);
                    cmd.Parameters.AddWithValue("@IsDeleted", false);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            result.Companies = readCompanyListFromDataReader(dr);
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }
        }

        public List<Company> LoadCompanies(string username, string password)
        {
            ValidateAuthorization(username, password);

            try
            {
                OpenConnection();
                List<Company> result = loadCompanies();
                CloseConnection();
                return result;
            }
            catch (Exception)
            {
                CloseConnection();
                throw;
            }
        }



        //DB repo methods
        private List<Company> loadCompanies()
        {
            List<Company> result = new List<Company>();

            string sql = "SELECT Id, Name, ImageName, IsDeleted, CreateDate FROM Companies WHERE IsDeleted = @IsDeleted ";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@IsDeleted", false);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        result = readCompanyListFromDataReader(dr);
                        return result;
                    }
                }
            }
            return result;
        }
        private Company getCompanyById(int id)
        {
            const string sql = "SELECT * FROM Companies WHERE Id = @Id AND IsDeleted = @IsDeleted";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@IsDeleted", false);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                        return readCompanyFromDataReader(dr);
                    else
                        return null;
                }
            }
        }
        private bool updateCompanyImage(int id, string imageName)
        {
            const string sql = "UPDATE Companies SET ImageName = @ImageName WHERE Id = @Id AND IsDeleted = @IsDeleted";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ImageName", CheckForDbNull(imageName));
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@IsDeleted", false);
                cmd.ExecuteNonQuery();
                return true;
            }
        }
        private bool checkCompanyExistByCompanyName(string name)
        {
            const string sql = "SELECT * FROM Companies WHERE Name = @Name AND IsDeleted = @IsDeleted";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@IsDeleted", false);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                        return true;
                    else
                        return false;
                }
            }
        }
        private bool checkCompanyExistByCompanyNameNotThis(string name, int id)
        {
            const string sql = "SELECT * FROM Companies WHERE Name = @Name AND IsDeleted = @IsDeleted AND Id != @Id";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@IsDeleted", false);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                        return true;
                    else
                        return false;
                }
            }
        }
        private Company readCompanyFromDataReader(SqlDataReader dr)
        {
            Company model = null;
            while (dr.Read())
            {
                model = new Company();
                model.Id = Convert.ToInt32(dr["Id"].ToString());
                model.Name = dr["Name"].ToString();
                model.Address = dr["Address"].ToString();
                model.Voen = dr["Voen"].ToString();
                model.Mail = dr["Mail"].ToString();
                model.Phone = dr["Phone"].ToString();
                model.CountryId = Convert.ToInt32(dr["CountryId"].ToString());
                model.CityId = Convert.ToInt32(dr["CityId"].ToString());
                model.IsDeleted = Convert.ToBoolean(dr["IsDeleted"].ToString());
                model.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());
            }
            return model;
        }
        private List<Company> readCompanyListFromDataReader(SqlDataReader dr)
        {
            List<Company> result = new List<Company>();
            while (dr.Read())
            {
                Company model = new Company();
                model.Id = Convert.ToInt32(dr["Id"].ToString());
                model.Name = dr["Name"].ToString();
                model.Address = dr["Address"].ToString();
                model.Voen = dr["Voen"].ToString();
                model.Mail = dr["Mail"].ToString();
                model.Phone = dr["Phone"].ToString();
                model.CountryId = Convert.ToInt32(dr["CountryId"].ToString());
                model.CityId = Convert.ToInt32(dr["CityId"].ToString());
                model.IsDeleted = Convert.ToBoolean(dr["IsDeleted"].ToString());
                model.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());
                result.Add(model);
            }
            return result;
        }
    }
}
