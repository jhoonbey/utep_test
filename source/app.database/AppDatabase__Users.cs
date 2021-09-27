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
        public bool CreateUser(User model, string username, string password)
        {
            ValidateAuthorization(username, password);
            Validator.UserCreateValidation(model);

            using (var t = new TransactionScope())
            {
                try
                {
                    OpenConnection();

                    //db validations
                    if (checkUserExistByUsername(model.Username)) throw new Exception("This username is busy");

                    string passwordCrypted = Reverse.E(model.Password);   //cyrpte password
                    const string sql = "INSERT INTO Users (Username, Password, Fullname, Role, IsDeleted, CreateDate ) VALUES" +
                                                         "(@Username, @Password, @Fullname, @Role, @IsDeleted, @CreateDate )";
                    using (var cmd = new SqlCommand(sql, _conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Username", model.Username);
                        cmd.Parameters.AddWithValue("@Password", passwordCrypted);
                        cmd.Parameters.AddWithValue("@Role", model.Role);
                        cmd.Parameters.AddWithValue("@Fullname", CheckForDbNull(model.Fullname));
                        cmd.Parameters.AddWithValue("@IsDeleted", false);
                        cmd.Parameters.AddWithValue("@CreateDate", GetDateTimeNow());
                        cmd.ExecuteNonQuery();
                    }

                    CloseConnection();
                    t.Complete();
                    return true;
                }
                catch (Exception)
                {
                    CloseConnection();
                    t.Dispose();
                    throw;
                }
            }
        }
        public bool EditUser(User model, string username, string password)
        {
            ValidateAuthorization(username, password);
            Validator.UserEditValidation(model);

            using (var t = new TransactionScope())
            {
                try
                {
                    OpenConnection();

                    //db validations
                    if (checkUserExistByUsernameNotThis(model.Username, model.Id)) throw new Exception("This username is busy");

                    const string sql = "UPDATE Users SET Username = @Username, Fullname = @Fullname, Role = @Role WHERE Id = @Id AND IsDeleted = @IsDeleted";
                    using (var cmd = new SqlCommand(sql, _conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Id", model.Id);
                        cmd.Parameters.AddWithValue("@Username", model.Username);
                        cmd.Parameters.AddWithValue("@Role", model.Role);
                        cmd.Parameters.AddWithValue("@Fullname", CheckForDbNull(model.Fullname));
                        cmd.Parameters.AddWithValue("@IsDeleted", false);
                        cmd.ExecuteNonQuery();
                    }

                    CloseConnection();
                    t.Complete();
                    return true;
                }
                catch (Exception)
                {
                    CloseConnection();
                    t.Dispose();
                    throw;
                }
            }
        }
        public bool DeleteUser(int id, string username, string password)
        {
            ValidateAuthorization(username, password);

            using (var t = new TransactionScope())
            {
                try
                {
                    OpenConnection();
                    const string sql = "UPDATE Users SET IsDeleted = @IsDeleted WHERE Id = @Id";
                    using (var cmd = new SqlCommand(sql, _conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@IsDeleted", true);
                        cmd.ExecuteNonQuery();
                    }

                    CloseConnection();
                    t.Complete();
                    return true;
                }
                catch (Exception)
                {
                    CloseConnection();
                    t.Dispose();
                    throw;
                }
            }
        }
        public string ResetUserPassword(int loggedUserId, int loggedUserRole, int userId, string username, string password)
        {
            ValidateAuthorization(username, password);

            using (var t = new TransactionScope())
            {
                try
                {
                    OpenConnection();

                    var user = getUserById(userId);
                    if (user == null) throw new Exception("User not found");

                    //role validatoion
                    if (loggedUserRole <= user.Role)
                    {
                        throw new Exception("You can not reset password of this user." + loggedUserRole.ToString() + " - "+ user.Role.ToString());
                    }

                    //set password
                    string newPassword = Common.CRP(6);
                    string newPasswordCrptd = Reverse.E(newPassword);

                    const string sql = "UPDATE Users SET Password = @Password WHERE Id = @Id AND IsDeleted = @IsDeleted";
                    using (var cmd = new SqlCommand(sql, _conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Password", newPasswordCrptd);
                        cmd.Parameters.AddWithValue("@Id", userId);
                        cmd.Parameters.AddWithValue("@IsDeleted", false);
                        cmd.ExecuteNonQuery();
                    }

                    CloseConnection();
                    t.Complete();
                    return newPassword;
                }
                catch (Exception)
                {
                    CloseConnection();
                    t.Dispose();
                    throw;
                }
            }
        }
        public string ChangeUserPassword(int loggedUserId, string oldPass, string newPass, string newPass2, string username, string password)
        {
            ValidateAuthorization(username, password);
            Validator.UserChangePasswordValidation(oldPass, newPass, newPass2);

            using (var t = new TransactionScope())
            {
                try
                {
                    OpenConnection();

                    var user = getUserById(loggedUserId);
                    if (user == null) throw new Exception("User not found");

                    string newPasswordCrypted = Reverse.E(newPass);
                    string oldPasswordCrypted = Reverse.E(oldPass);

                    if (!checkUserExistByIdAndPassword(user.Id, oldPasswordCrypted)) throw new Exception("Incorrect old password");

                    const string sql = "UPDATE Users SET Password = @Password WHERE Id = @Id AND IsDeleted = @IsDeleted";
                    using (var cmd = new SqlCommand(sql, _conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Password", newPasswordCrypted);
                        cmd.Parameters.AddWithValue("@Id", user.Id);
                        cmd.Parameters.AddWithValue("@IsDeleted", false);
                        cmd.ExecuteNonQuery();
                    }

                    CloseConnection();
                    t.Complete();
                    return newPass;
                }
                catch (Exception)
                {
                    CloseConnection();
                    t.Dispose();
                    throw;
                }
            }
        }
        internal bool ValidateAuthorization(string username, string password)
        {
            try
            {
                OpenConnection();

                if (!checkUserExistByUsernameAndPassword(username, password))
                {
                    throw new Exception("Unauthorized user!!!");
                }

                CloseConnection();

                return true;
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

        public User GetUserByIdentification(string username, string password, int role)
        {
            Validator.UserIdentificationValidation(username, password, role);

            User result = null;

            try
            {

                OpenConnection();
                string passwordCrptd = Reverse.E(password);
                const string sql = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password AND Role = @Role AND IsDeleted = @IsDeleted";

                using (var cmd = new SqlCommand(sql, _conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", passwordCrptd);
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.Parameters.AddWithValue("@IsDeleted", false);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            result = readUserFromDataReader(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }

            if (result == null) throw new Exception("User not found");

            return result;
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            Validator.UserLoginValidation(username, password);
            User result = null;

            try
            {
                OpenConnection();

                string passwordCrptd = Reverse.E(password);
                const string sql = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password AND IsDeleted = @IsDeleted";

                using (var cmd = new SqlCommand(sql, _conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", passwordCrptd);
                    cmd.Parameters.AddWithValue("@IsDeleted", false);
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            result = readUserFromDataReader(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                CloseConnection();
            }

            if (result == null) throw new Exception("User not found");

            return result;
        }

        public User GetUserById(int id, string username, string password)
        {
            ValidateAuthorization(username, password);

            try
            {
                OpenConnection();

                return getUserById(id);
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
        public UserEntityCollection LoadUsersByCriteria(UserCriteriaModel criteria, int rowsPerPage, int pageNumber, string username, string password)
        {
            OpenConnection();

            UserEntityCollection result = new UserEntityCollection();
            result.Users = new List<User>();

            string _keyword = string.IsNullOrEmpty(criteria.Keyword) ? string.Empty : " AND ( Username LIKE @Username OR Fullname LIKE @Fullname ) ";
            string _role = criteria.Role == 0 ? string.Empty : " AND Role = @Role ";
            string _minCreateDate = (criteria.MinCreateDate == null || criteria.MinCreateDate.Value == DateTime.MinValue) ? string.Empty : " AND CreateDate >= @MinCreateDate ";
            string _maxCreateDate = (criteria.MaxCreateDate == null || criteria.MaxCreateDate.Value == DateTime.MinValue) ? string.Empty : " AND CreateDate <= @MaxCreateDate ";

            //calculate count
            string sqlCount = "SELECT COUNT(*) FROM Users WHERE IsDeleted = @IsDeleted " + _keyword + _role + _minCreateDate + _maxCreateDate;
            using (var cmdCount = new SqlCommand(sqlCount, _conn))
            {
                cmdCount.CommandType = CommandType.Text;
                if (!string.IsNullOrEmpty(_keyword))
                {
                    cmdCount.Parameters.AddWithValue("@Username", "%" + criteria.Keyword + "%");
                    cmdCount.Parameters.AddWithValue("@Fullname", "%" + criteria.Keyword + "%");
                }
                if (!string.IsNullOrEmpty(_role)) { cmdCount.Parameters.AddWithValue("@Role", criteria.Role); }
                if (!string.IsNullOrEmpty(_minCreateDate)) { cmdCount.Parameters.AddWithValue("@MinCreateDate", criteria.MinCreateDate); }
                if (!string.IsNullOrEmpty(_maxCreateDate)) { cmdCount.Parameters.AddWithValue("@MaxCreateDate", criteria.MaxCreateDate); }
                cmdCount.Parameters.AddWithValue("@IsDeleted", false);
                result.AllCount = Convert.ToInt32(cmdCount.ExecuteScalar());
            }

            //calculate list
            string sql = "SELECT Id, Username, Password, Fullname, Role, IsDeleted, CreateDate FROM ( " +
                         "SELECT Id, Username, Password, Fullname, Role, IsDeleted, CreateDate, " +
                         "ROW_NUMBER() OVER (ORDER BY CreateDate DESC) AS RowNum FROM Users WHERE IsDeleted = @IsDeleted " +
                         _keyword + _role + _minCreateDate + _maxCreateDate +
                         " ) AS SOD WHERE SOD.RowNum BETWEEN @RowStart AND @RowEnd ORDER BY CreateDate DESC";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                if (!string.IsNullOrEmpty(_keyword))
                {
                    cmd.Parameters.AddWithValue("@Username", "%" + criteria.Keyword + "%");
                    cmd.Parameters.AddWithValue("@Fullname", "%" + criteria.Keyword + "%");
                }
                if (!string.IsNullOrEmpty(_role)) { cmd.Parameters.AddWithValue("@Role", criteria.Role); }
                if (!string.IsNullOrEmpty(_minCreateDate)) { cmd.Parameters.AddWithValue("@MinCreateDate", criteria.MinCreateDate); }
                if (!string.IsNullOrEmpty(_maxCreateDate)) { cmd.Parameters.AddWithValue("@MaxCreateDate", criteria.MaxCreateDate); }

                cmd.Parameters.AddWithValue("@RowStart", ((pageNumber - 1) * rowsPerPage) + 1);
                cmd.Parameters.AddWithValue("@RowEnd", rowsPerPage * pageNumber);
                cmd.Parameters.AddWithValue("@IsDeleted", false);

                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        result.Users = readUserListFromDataReader(dr);

                        CloseConnection();
                        return result;
                    }
                }
            }

            CloseConnection();
            return result;
        }
        public List<EnumModel> LoadAllUserRoleEnumsWithout(List<int> idList, string username, string password)
        {
            List<EnumModel> result = new List<EnumModel>();
            var all = Enum.GetValues(typeof(EnumUserRole));
            foreach (var value in all)
            {
                int id = (int)value;
                EnumUserRole enumOwn = (EnumUserRole)value;
                if (!idList.Contains(id))
                {
                    result.Add(new EnumModel() { Id = id, Name = enumOwn.ToString() });
                }
            }
            return result;
        }


        //user valdiation
        internal bool checkUserExistByUsernameAndPassword(string username, string password)
        {
            const string sql = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password AND IsDeleted = @IsDeleted";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", Reverse.E(password));
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

        //DB repo methods
        private User getUserById(int id)
        {
            const string sql = "SELECT * FROM Users WHERE Id = @Id AND IsDeleted = @IsDeleted";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@IsDeleted", false);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                        return readUserFromDataReader(dr);
                    else
                        return null;
                }
            }
        }
        private bool checkUserExistByIdAndPassword(int id, string password)
        {
            const string sql = "SELECT * FROM Users WHERE Password = @Password AND Id = @Id AND IsDeleted = @IsDeleted";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Password", password);
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
        private bool checkUserExistByUsername(string username)
        {
            const string sql = "SELECT * FROM Users WHERE Username = @Username AND IsDeleted = @IsDeleted";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Username", username);
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
        private bool checkUserExistByUsernameNotThis(string username, int id)
        {
            const string sql = "SELECT * FROM Users WHERE Username = @Username AND IsDeleted = @IsDeleted AND Id != @Id";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Username", username);
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
        private User readUserFromDataReader(SqlDataReader dr)
        {
            User model = null;
            while (dr.Read())
            {
                model = new User();
                model.Id = Convert.ToInt32(dr["Id"].ToString());
                model.Username = dr["Username"].ToString();
                model.Password = Reverse.D(dr["Password"].ToString());
                model.Fullname = dr["Fullname"].ToString();
                model.Role = Convert.ToInt32(dr["Role"].ToString());
                model.IsDeleted = Convert.ToBoolean(dr["IsDeleted"].ToString());
                model.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());
            }
            return model;
        }
        private List<User> readUserListFromDataReader(SqlDataReader dr)
        {
            List<User> result = new List<User>();
            while (dr.Read())
            {
                User model = new User();
                model.Id = Convert.ToInt32(dr["Id"].ToString());
                model.Username = dr["Username"].ToString();
                model.Password = Reverse.D(dr["Password"].ToString());
                model.Fullname = dr["Fullname"].ToString();
                model.Role = Convert.ToInt32(dr["Role"].ToString());
                model.IsDeleted = Convert.ToBoolean(dr["IsDeleted"].ToString());
                model.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());
                result.Add(model);
            }
            return result;
        }
    }
}
