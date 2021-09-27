using app.database.Validations;
using app.domain.Model.Entities;
using app.Model.Criterias;
using app.domain.Model.EntityCollections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace app.database
{
    public partial class AppDatabase
    {
        public bool CreateCountry(Country model, string username, string password)
        {
            ValidateAuthorization(username, password);
            Validator.CountryCreateValidation(model);


            try
            {
                OpenConnection();

                //db validations
                if (checkCountryExistByCountryName(model.Name)) throw new Exception("This Name is busy");

                const string sql = "INSERT INTO Countries (Name,  IsDeleted,  CreateDate ) VALUES" +
                                                        "(@Name, @IsDeleted, @CreateDate)";
                using (var cmd = new SqlCommand(sql, _conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", model.Name);
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
        public bool EditCountry(Country model, string username, string password)
        {
            ValidateAuthorization(username, password);
            Validator.CountryEditValidation(model);

            try
            {
                OpenConnection();

                //db validations
                if (checkCountryExistByCountryNameNotThis(model.Name, model.Id)) throw new Exception("This Name is busy");

                const string sql = "UPDATE Countries SET Name = @Name WHERE Id = @Id AND IsDeleted = @IsDeleted";
                using (var cmd = new SqlCommand(sql, _conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Name", model.Name);
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
        //public bool DeleteCountry(int id, string username, string password)
        //{
        //    using (var t = new TransactionScope())
        //    {
        //        try
        //        {
        //            OpenConnection();

        //            var category = getCountryById(id);
        //            if (category == null) throw new Exception("Country not found");
        //            string imageName = category.ImageName;

        //            if (checkProductExistByCountry(category.Id)) throw new Exception("Device Type has a Device(s). Firstly, delete it's device(s)");

        //            const string sql = "UPDATE Countries SET IsDeleted = @IsDeleted WHERE Id = @Id";
        //            using (var cmd = new SqlCommand(sql, _conn))
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                cmd.Parameters.AddWithValue("@Id", id);
        //                cmd.Parameters.AddWithValue("@IsDeleted", true);
        //                cmd.ExecuteNonQuery();
        //            }


        //            //delete image
        //            MediaHelper.DeleteImageFromFolder(imageName, "Country");


        //            CloseConnection();
        //            t.Complete();
        //            return true;
        //        }
        //        catch (Exception)
        //        {
        //            CloseConnection();
        //            t.Dispose();
        //            throw;
        //        }
        //    }
        //}


        //get methods
        public Country GetCountryById(int id, string username, string password)
        {
            ValidateAuthorization(username, password);

            try
            {
                OpenConnection();

                return getCountryById(id); 
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

        //public CountryViewModel GetCountryViewModel(int categoryId, string username, string password)
        //{
        //    try
        //    {
        //        OpenConnection();
        //        CountryViewModel model = new CountryViewModel();
        //        model.Country = getCountryById(categoryId);

        //        if (model.Country == null)
        //            throw new Exception("Country not found");

        //        model.Products = loadProductsByCriteria(new ProductCriteriaModel { CountryId = categoryId }, 10000, 1);
        //        CloseConnection();
        //        return model;
        //    }
        //    catch (Exception)
        //    {
        //        CloseConnection();
        //        throw;
        //    }
        //}

        public CountryEntityCollection LoadCountriesByCriteria(CountryCriteriaModel criteria, int rowsPerPage, int pageNumber, string username, string password)
        {
            ValidateAuthorization(username, password);

            try
            {
                OpenConnection();

                CountryEntityCollection result = new CountryEntityCollection();
                result.Countries = new List<Country>();

                string _minCreateDate = (criteria.MinCreateDate == null || criteria.MinCreateDate.Value == DateTime.MinValue) ? string.Empty : " AND CreateDate >= @MinCreateDate ";
                string _maxCreateDate = (criteria.MaxCreateDate == null || criteria.MaxCreateDate.Value == DateTime.MinValue) ? string.Empty : " AND CreateDate <= @MaxCreateDate ";

                //calculate count
                string sqlCount = "SELECT COUNT(*) FROM Countries WHERE IsDeleted = @IsDeleted " + _minCreateDate + _maxCreateDate;
                using (var cmdCount = new SqlCommand(sqlCount, _conn))
                {
                    cmdCount.CommandType = CommandType.Text;
                    if (!string.IsNullOrEmpty(_minCreateDate)) { cmdCount.Parameters.AddWithValue("@MinCreateDate", criteria.MinCreateDate); }
                    if (!string.IsNullOrEmpty(_maxCreateDate)) { cmdCount.Parameters.AddWithValue("@MaxCreateDate", criteria.MaxCreateDate); }
                    cmdCount.Parameters.AddWithValue("@IsDeleted", false);
                    result.AllCount = Convert.ToInt32(cmdCount.ExecuteScalar());
                }

                //calculate list
                string sql = "SELECT Id, Name, IsDeleted, CreateDate FROM ( " +
                             "SELECT Id, Name, IsDeleted, CreateDate, " +
                             "ROW_NUMBER() OVER (ORDER BY CreateDate DESC) AS RowNum FROM Countries WHERE IsDeleted = @IsDeleted " +
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
                            result.Countries = readCountryListFromDataReader(dr);

                            CloseConnection();
                            return result;
                        }
                    }
                }

                return result;
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
        public List<Country> LoadCountries(string username, string password)
        {
            ValidateAuthorization(username, password);

            try
            {
                OpenConnection();
                List<Country> result = loadCountries();
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
        private List<Country> loadCountries()
        {
            List<Country> result = new List<Country>();

            string sql = "SELECT Id, Name, ImageName, IsDeleted, CreateDate FROM Countries WHERE IsDeleted = @IsDeleted ";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@IsDeleted", false);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        result = readCountryListFromDataReader(dr);
                        return result;
                    }
                }
            }
            return result;
        }
        private Country getCountryById(int id)
        {
            const string sql = "SELECT * FROM Countries WHERE Id = @Id AND IsDeleted = @IsDeleted";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@IsDeleted", false);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                        return readCountryFromDataReader(dr);
                    else
                        return null;
                }
            }
        }
       
        private bool checkCountryExistByCountryName(string name)
        {
            const string sql = "SELECT * FROM Countries WHERE Name = @Name AND IsDeleted = @IsDeleted";
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
        private bool checkCountryExistByCountryNameNotThis(string name, int id)
        {
            const string sql = "SELECT * FROM Countries WHERE Name = @Name AND IsDeleted = @IsDeleted AND Id != @Id";
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
        private Country readCountryFromDataReader(SqlDataReader dr)
        {
            Country model = null;
            while (dr.Read())
            {
                model = new Country();
                model.Id = Convert.ToInt32(dr["Id"].ToString());
                model.Name = dr["Name"].ToString();
                model.IsDeleted = Convert.ToBoolean(dr["IsDeleted"].ToString());
                model.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());
            }
            return model;
        }
        private List<Country> readCountryListFromDataReader(SqlDataReader dr)
        {
            List<Country> result = new List<Country>();
            while (dr.Read())
            {
                Country model = new Country();
                model.Id = Convert.ToInt32(dr["Id"].ToString());
                model.Name = dr["Name"].ToString();
                model.IsDeleted = Convert.ToBoolean(dr["IsDeleted"].ToString());
                model.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());
                result.Add(model);
            }
            return result;
        }
    }
}

