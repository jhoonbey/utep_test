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
        public bool CreateTransportType(TransportType model, string username, string password)
        {
            ValidateAuthorization(username, password);
            Validator.TransportTypeCreateValidation(model);

            try
            {
                OpenConnection();

                //db validations
                if (checkTransportTypeExistByTransportTypeName(model.Name)) throw new Exception("This Name is busy");

                const string sql = "INSERT INTO TransportTypes (Name,  WeightCapacity,  VolumeCapacity,  CapacityInPalletes,  Description,  IsDeleted,  CreateDate ) VALUES" +
                                                             "(@Name, @WeightCapacity, @VolumeCapacity, @CapacityInPalletes, @Description, @IsDeleted, @CreateDate)";

                using (var cmd = new SqlCommand(sql, _conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", model.Name);
                    cmd.Parameters.AddWithValue("@WeightCapacity", CheckForDbNull(model.WeightCapacity));
                    cmd.Parameters.AddWithValue("@VolumeCapacity", CheckForDbNull(model.VolumeCapacity));
                    cmd.Parameters.AddWithValue("@CapacityInPalletes", CheckForDbNull(model.CapacityInPalletes));
                    cmd.Parameters.AddWithValue("@Description", CheckForDbNull(model.Description));
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

        public bool EditTransportType(TransportType model, string username, string password)
        {
            ValidateAuthorization(username, password);
            Validator.TransportTypeEditValidation(model);


            try
            {
                OpenConnection();

                //db validations
                if (checkTransportTypeExistByTransportTypeNameNotThis(model.Name, model.Id)) throw new Exception("This Name is busy");

                const string sql = "UPDATE TransportTypes SET Name = @Name,  WeightCapacity = @WeightCapacity,  VolumeCapacity = @VolumeCapacity,  CapacityInPalletes = @CapacityInPalletes,  " +
                                                             "Description = @Description WHERE Id = @Id AND IsDeleted = @IsDeleted";

                using (var cmd = new SqlCommand(sql, _conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Name", model.Name);
                    cmd.Parameters.AddWithValue("@WeightCapacity", CheckForDbNull(model.WeightCapacity));
                    cmd.Parameters.AddWithValue("@VolumeCapacity", CheckForDbNull(model.VolumeCapacity));
                    cmd.Parameters.AddWithValue("@CapacityInPalletes", CheckForDbNull(model.CapacityInPalletes));
                    cmd.Parameters.AddWithValue("@Description", CheckForDbNull(model.Description));

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

        public bool DeleteTransportType(int id, string username, string password)
        {
            ValidateAuthorization(username, password);

            try
            {
                OpenConnection();

                var model = getTransportTypeById(id);
                if (model == null) throw new Exception("Transport type not found");

                if (checkSupplierExistBy("TransportTypeId", model.Id)) throw new Exception("This Transport type is related a Supplier. Firstly, delete it.");

                const string sql = "UPDATE TransportTypes SET IsDeleted = @IsDeleted WHERE Id = @Id";
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
        public TransportType GetTransportTypeById(int id, string username, string password)
        {
            ValidateAuthorization(username, password);

            try
            {
                OpenConnection();

                return getTransportTypeById(id);
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

        //public TransportTypeViewModel GetTransportTypeViewModel(int categoryId, string username, string password)
        //{
        //    try
        //    {
        //        OpenConnection();
        //        TransportTypeViewModel model = new TransportTypeViewModel();
        //        model.TransportType = getTransportTypeById(categoryId);

        //        if (model.TransportType == null)
        //            throw new Exception("TransportType not found");

        //        model.Products = loadProductsByCriteria(new ProductCriteriaModel { TransportTypeId = categoryId }, 10000, 1);
        //        CloseConnection();
        //        return model;
        //    }
        //    catch (Exception)
        //    {
        //        CloseConnection();
        //        throw;
        //    }
        //}


        public TransportTypeEntityCollection LoadTransportTypesByCriteria(TransportTypeCriteriaModel criteria, int rowsPerPage, int pageNumber, string username, string password)
        {
            ValidateAuthorization(username, password);

            try
            {
                OpenConnection();

                TransportTypeEntityCollection result = new TransportTypeEntityCollection();
                result.TransportTypes = new List<TransportType>();

                string _minCreateDate = (criteria.MinCreateDate == null || criteria.MinCreateDate.Value == DateTime.MinValue) ? string.Empty : " AND CreateDate >= @MinCreateDate ";
                string _maxCreateDate = (criteria.MaxCreateDate == null || criteria.MaxCreateDate.Value == DateTime.MinValue) ? string.Empty : " AND CreateDate <= @MaxCreateDate ";

                //calculate count
                string sqlCount = "SELECT COUNT(*) FROM TransportTypes WHERE IsDeleted = @IsDeleted " + _minCreateDate + _maxCreateDate;
                using (var cmdCount = new SqlCommand(sqlCount, _conn))
                {
                    cmdCount.CommandType = CommandType.Text;
                    if (!string.IsNullOrEmpty(_minCreateDate)) { cmdCount.Parameters.AddWithValue("@MinCreateDate", criteria.MinCreateDate); }
                    if (!string.IsNullOrEmpty(_maxCreateDate)) { cmdCount.Parameters.AddWithValue("@MaxCreateDate", criteria.MaxCreateDate); }
                    cmdCount.Parameters.AddWithValue("@IsDeleted", false);
                    result.AllCount = Convert.ToInt32(cmdCount.ExecuteScalar());
                }

                //calculate list
                string sql = "SELECT Id, Name,  WeightCapacity,  VolumeCapacity,  CapacityInPalletes,  Description,  IsDeleted,  CreateDate FROM ( " +
                             "SELECT Id, Name,  WeightCapacity,  VolumeCapacity,  CapacityInPalletes,  Description,  IsDeleted,  CreateDate, " +
                             "ROW_NUMBER() OVER (ORDER BY CreateDate DESC) AS RowNum FROM TransportTypes WHERE IsDeleted = @IsDeleted " + 
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
                            result.TransportTypes = readTransportTypeListFromDataReader(dr);
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

        public List<TransportType> LoadTransportTypes(string username, string password)
        {
            ValidateAuthorization(username, password);

            try
            {
                OpenConnection();
                List<TransportType> result = loadTransportTypes();
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
        private List<TransportType> loadTransportTypes()
        {
            List<TransportType> result = new List<TransportType>();

            string sql = "SELECT Id, Name, ImageName, IsDeleted, CreateDate FROM TransportTypes WHERE IsDeleted = @IsDeleted ";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@IsDeleted", false);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        result = readTransportTypeListFromDataReader(dr);
                        return result;
                    }
                }
            }
            return result;
        }
        private TransportType getTransportTypeById(int id)
        {
            const string sql = "SELECT * FROM TransportTypes WHERE Id = @Id AND IsDeleted = @IsDeleted";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@IsDeleted", false);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                        return readTransportTypeFromDataReader(dr);
                    else
                        return null;
                }
            }
        }
        private bool checkTransportTypeExistByTransportTypeName(string name)
        {
            const string sql = "SELECT * FROM TransportTypes WHERE Name = @Name AND IsDeleted = @IsDeleted";
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
        private bool checkTransportTypeExistByTransportTypeNameNotThis(string name, int id)
        {
            const string sql = "SELECT * FROM TransportTypes WHERE Name = @Name AND IsDeleted = @IsDeleted AND Id != @Id";
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
        private TransportType readTransportTypeFromDataReader(SqlDataReader dr)
        {
            TransportType model = null;
            while (dr.Read())
            {
                model = new TransportType();
                model.Id = Convert.ToInt32(dr["Id"].ToString());
                model.Name = dr["Name"].ToString();
                model.WeightCapacity = dr["WeightCapacity"].ToString();
                model.VolumeCapacity = dr["VolumeCapacity"].ToString();
                model.CapacityInPalletes = dr["CapacityInPalletes"].ToString();
                model.Description = dr["Description"].ToString();
                model.IsDeleted = Convert.ToBoolean(dr["IsDeleted"].ToString());
                model.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());
            }
            return model;
        }
        private List<TransportType> readTransportTypeListFromDataReader(SqlDataReader dr)
        {
            List<TransportType> result = new List<TransportType>();
            while (dr.Read())
            {
                TransportType model = new TransportType();
                model.Id = Convert.ToInt32(dr["Id"].ToString());
                model.Name = dr["Name"].ToString();
                model.WeightCapacity = dr["WeightCapacity"].ToString();
                model.VolumeCapacity = dr["VolumeCapacity"].ToString();
                model.CapacityInPalletes = dr["CapacityInPalletes"].ToString();
                model.Description = dr["Description"].ToString();
                model.IsDeleted = Convert.ToBoolean(dr["IsDeleted"].ToString());
                model.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());
                result.Add(model);
            }
            return result;
        }
    }
}
