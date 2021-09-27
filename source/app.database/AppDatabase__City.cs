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
        public bool CreateCity(City model, string username, string password)
        {
            ValidateAuthorization(username, password);
            Validator.CityCreateValidation(model);

            try
            {
                OpenConnection();

                //db validations
                if (checkCityExistByCityName(model.Name)) throw new Exception("This Name is busy");

                const string sql = "INSERT INTO Cities (Name,  CountryId,  IsDeleted,  CreateDate ) VALUES" +
                                                     "(@Name, @CountryId, @IsDeleted, @CreateDate)";

                using (var cmd = new SqlCommand(sql, _conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", model.Name);
                    cmd.Parameters.AddWithValue("@CountryId", model.CountryId);

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


        public bool EditCity(City model, string username, string password)
        {
            ValidateAuthorization(username, password);
            Validator.CityEditValidation(model);


            try
            {
                OpenConnection();

                //db validations
                if (checkCityExistByCityNameNotThis(model.Name, model.Id)) throw new Exception("This Name is busy");

                const string sql = "UPDATE Cities SET Name = @Name,  CountryId = @CountryId WHERE Id = @Id AND IsDeleted = @IsDeleted";

                using (var cmd = new SqlCommand(sql, _conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Name", model.Name);
                    cmd.Parameters.AddWithValue("@CountryId", model.CountryId);

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

        //public bool DeleteCity(int id, string username, string password, string username, string password)
        //{
        //    using (var t = new TransactionScope())
        //    {
        //        try
        //        {
        //            OpenConnection();

        //            var category = getCityById(id);
        //            if (category == null) throw new Exception("City not found");
        //            //string imageName = category.ImageName;

        //            //if (checkProductExistByCity(category.Id)) throw new Exception("city  a Device(s). Firstly, delete it's device(s)");

        //            const string sql = "UPDATE Cities SET IsDeleted = @IsDeleted WHERE Id = @Id";
        //            using (var cmd = new SqlCommand(sql, _conn))
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                cmd.Parameters.AddWithValue("@Id", id);
        //                cmd.Parameters.AddWithValue("@IsDeleted", true);
        //                cmd.ExecuteNonQuery();
        //            }


        //            //delete image
        //            MediaHelper.DeleteImageFromFolder(imageName, "City");


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
        public City GetCityById(int id, string username, string password)
        {
            ValidateAuthorization(username, password);

            try
            {
                OpenConnection();

                return getCityById(id);
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

        //public CityViewModel GetCityViewModel(int categoryId, string username, string password)
        //{
        //    try
        //    {
        //        OpenConnection();
        //        CityViewModel model = new CityViewModel();
        //        model.City = getCityById(categoryId);

        //        if (model.City == null)
        //            throw new Exception("City not found");

        //        model.Products = loadProductsByCriteria(new ProductCriteriaModel { CityId = categoryId }, 10000, 1);
        //        CloseConnection();
        //        return model;
        //    }
        //    catch (Exception)
        //    {
        //        CloseConnection();
        //        throw;
        //    }
        //}


        public CityEntityCollection LoadCitiesByCriteria(CityCriteriaModel criteria, int rowsPerPage, int pageNumber, string username, string password)
        {
            ValidateAuthorization(username, password);

            try
            {
                OpenConnection();

                CityEntityCollection result = new CityEntityCollection();
                result.Cities = new List<City>();

                string _countryId = criteria.CountryId == 0 ? string.Empty : " AND CountryId = @CountryId ";
                string _minCreateDate = (criteria.MinCreateDate == null || criteria.MinCreateDate.Value == DateTime.MinValue) ? string.Empty : " AND CreateDate >= @MinCreateDate ";
                string _maxCreateDate = (criteria.MaxCreateDate == null || criteria.MaxCreateDate.Value == DateTime.MinValue) ? string.Empty : " AND CreateDate <= @MaxCreateDate ";

                //calculate count
                string sqlCount = "SELECT COUNT(*) FROM Cities WHERE IsDeleted = @IsDeleted " + _minCreateDate + _maxCreateDate + _countryId;
                using (var cmdCount = new SqlCommand(sqlCount, _conn))
                {
                    cmdCount.CommandType = CommandType.Text;
                    if (!string.IsNullOrEmpty(_countryId)) { cmdCount.Parameters.AddWithValue("@CountryId", criteria.CountryId); }
                    if (!string.IsNullOrEmpty(_minCreateDate)) { cmdCount.Parameters.AddWithValue("@MinCreateDate", criteria.MinCreateDate); }
                    if (!string.IsNullOrEmpty(_maxCreateDate)) { cmdCount.Parameters.AddWithValue("@MaxCreateDate", criteria.MaxCreateDate); }
                    cmdCount.Parameters.AddWithValue("@IsDeleted", false);
                    result.AllCount = Convert.ToInt32(cmdCount.ExecuteScalar());
                }

                //calculate list
                string sql = "SELECT Id, Name,  CountryId,  IsDeleted,  CreateDate FROM ( " +
                             "SELECT Id, Name,  CountryId,  IsDeleted,  CreateDate, " +
                             "ROW_NUMBER() OVER (ORDER BY CreateDate DESC) AS RowNum FROM Cities WHERE IsDeleted = @IsDeleted " +
                             _minCreateDate + _maxCreateDate + _countryId +
                             " ) AS SOD WHERE SOD.RowNum BETWEEN @RowStart AND @RowEnd ORDER BY CreateDate DESC";
                using (var cmd = new SqlCommand(sql, _conn))
                {
                    cmd.CommandType = CommandType.Text;
                    if (!string.IsNullOrEmpty(_countryId)) { cmd.Parameters.AddWithValue("@CountryId", criteria.CountryId); }
                    if (!string.IsNullOrEmpty(_minCreateDate)) { cmd.Parameters.AddWithValue("@MinCreateDate", criteria.MinCreateDate); }
                    if (!string.IsNullOrEmpty(_maxCreateDate)) { cmd.Parameters.AddWithValue("@MaxCreateDate", criteria.MaxCreateDate); }
                    cmd.Parameters.AddWithValue("@RowStart", ((pageNumber - 1) * rowsPerPage) + 1);
                    cmd.Parameters.AddWithValue("@RowEnd", rowsPerPage * pageNumber);
                    cmd.Parameters.AddWithValue("@IsDeleted", false);

                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            result.Cities = readCityListFromDataReader(dr);
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

        public List<City> LoadCities(string username, string password)
        {
            ValidateAuthorization(username, password);

            try
            {
                OpenConnection();
                List<City> result = loadCities();
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
        private List<City> loadCities()
        {
            List<City> result = new List<City>();

            string sql = "SELECT Id, Name, ImageName, IsDeleted, CreateDate FROM Cities WHERE IsDeleted = @IsDeleted ";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@IsDeleted", false);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        result = readCityListFromDataReader(dr);
                        return result;
                    }
                }
            }
            return result;
        }
        private City getCityById(int id)
        {
            const string sql = "SELECT * FROM Cities WHERE Id = @Id AND IsDeleted = @IsDeleted";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@IsDeleted", false);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                        return readCityFromDataReader(dr);
                    else
                        return null;
                }
            }
        }
        //private bool updateCityImage(int id, string imageName)
        //{
        //    const string sql = "UPDATE Cities SET ImageName = @ImageName WHERE Id = @Id AND IsDeleted = @IsDeleted";
        //    using (var cmd = new SqlCommand(sql, _conn))
        //    {
        //        cmd.CommandType = CommandType.Text;
        //        cmd.Parameters.AddWithValue("@ImageName", CheckForDbNull(imageName));
        //        cmd.Parameters.AddWithValue("@Id", id);
        //        cmd.Parameters.AddWithValue("@IsDeleted", false);
        //        cmd.ExecuteNonQuery();
        //        return true;
        //    }
        //}
        private bool checkCityExistByCityName(string name)
        {
            const string sql = "SELECT * FROM Cities WHERE Name = @Name AND IsDeleted = @IsDeleted";
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
        private bool checkCityExistByCityNameNotThis(string name, int id)
        {
            const string sql = "SELECT * FROM Cities WHERE Name = @Name AND IsDeleted = @IsDeleted AND Id != @Id";
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
        private City readCityFromDataReader(SqlDataReader dr)
        {
            City model = null;
            while (dr.Read())
            {
                model = new City();
                model.Id = Convert.ToInt32(dr["Id"].ToString());
                model.Name = dr["Name"].ToString();
                model.CountryId = Convert.ToInt32(dr["CountryId"].ToString());
                model.IsDeleted = Convert.ToBoolean(dr["IsDeleted"].ToString());
                model.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());
            }
            return model;
        }
        private List<City> readCityListFromDataReader(SqlDataReader dr)
        {
            List<City> result = new List<City>();
            while (dr.Read())
            {
                City model = new City();
                model.Id = Convert.ToInt32(dr["Id"].ToString());
                model.Name = dr["Name"].ToString();
                model.CountryId = Convert.ToInt32(dr["CountryId"].ToString());
                model.IsDeleted = Convert.ToBoolean(dr["IsDeleted"].ToString());
                model.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());
                result.Add(model);
            }
            return result;
        }
    }
}
