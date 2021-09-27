using app.database.Validations;
using app.domain.Model.Data;
using app.domain.Model.Entities;
using app.Model.Criterias;
using app.domain.Model.EntityCollections;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DataManager;
using System.IO;
using System.Transactions;
using System.Web;

namespace app.database
{
    public partial class AppDatabase
    {
        public bool CreateSupplier(Supplier model, HttpPostedFileBase inter, HttpPostedFileBase lic, HttpPostedFileBase pass, HttpPostedFileBase pass2,
                                                   HttpPostedFileBase front, HttpPostedFileBase back, HttpPostedFileBase right, HttpPostedFileBase left,
                                   string username, string password)
        {
            ValidateAuthorization(username, password);
            Validator.SupplierCreateValidation(model, inter, lic, pass, pass2, front, back, right, left);

            using (var t = new TransactionScope())
            {
                try
                {
                    OpenConnection();

                    int insertedId = 0;
                    const string sql = @"INSERT INTO Suppliers (  FirstName 		
                                                                , LastName
                                                                , WhatsappNumber
                                                                , ContactNumber
                                                                , PlateNumber
                                                                , PlateNumber2
                                                                , TransportPassport
                                                                , TransportPassport2
                                                                , InternationalPassport
                                                                , DrivingLicense
                                                                , TransportPassportPhotoName
                                                                , TransportPassport2PhotoName
                                                                , InternationalPassportPhotoName
                                                                , DrivingLicensePhotoName
                                                                , Year
                                                                , Condition
                                                                , TruckFrontPhotoName
                                                                , TruckBackPhotoName
                                                                , TruckRightPhotoName
                                                                , TruckLeftPhotoName
                                                                , TransportTypeId
                                                                , CompanyId, IsDeleted,  CreateDate ) VALUES(
                    		                                     @FirstName
                                                                ,@LastName
                                                                ,@WhatsappNumber
                                                                ,@ContactNumber
                                                                ,@PlateNumber
                                                                ,@PlateNumber2
                                                                ,@TransportPassport
                                                                ,@TransportPassport2
                                                                ,@InternationalPassport
                                                                ,@DrivingLicense
                                                                ,@TransportPassportPhotoName
                                                                ,@TransportPassport2PhotoName
                                                                ,@InternationalPassportPhotoName
                                                                ,@DrivingLicensePhotoName
                                                                ,@Year
                                                                ,@Condition
                                                                ,@TruckFrontPhotoName
                                                                ,@TruckBackPhotoName
                                                                ,@TruckRightPhotoName
                                                                ,@TruckLeftPhotoName
                                                                ,@TransportTypeId
                                                                ,@CompanyId, @IsDeleted, @CreateDate); SELECT SCOPE_IDENTITY();";

                    using (var cmd = new SqlCommand(sql, _conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@FirstName", model.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", model.LastName);

                        cmd.Parameters.AddWithValue("@WhatsappNumber", CheckForDbNull(model.WhatsappNumber));
                        cmd.Parameters.AddWithValue("@ContactNumber", CheckForDbNull(model.ContactNumber));
                        cmd.Parameters.AddWithValue("@PlateNumber", CheckForDbNull(model.PlateNumber));
                        cmd.Parameters.AddWithValue("@PlateNumber2", CheckForDbNull(model.PlateNumber2));
                        cmd.Parameters.AddWithValue("@TransportPassport", CheckForDbNull(model.TransportPassport));
                        cmd.Parameters.AddWithValue("@TransportPassport2", CheckForDbNull(model.TransportPassport2));
                        cmd.Parameters.AddWithValue("@InternationalPassport", CheckForDbNull(model.InternationalPassport));
                        cmd.Parameters.AddWithValue("@DrivingLicense", CheckForDbNull(model.DrivingLicense));

                        cmd.Parameters.AddWithValue("@TransportPassportPhotoName", CheckForDbNull(model.TransportPassportPhotoName));
                        cmd.Parameters.AddWithValue("@TransportPassport2PhotoName", CheckForDbNull(model.TransportPassport2PhotoName));
                        cmd.Parameters.AddWithValue("@InternationalPassportPhotoName", CheckForDbNull(model.InternationalPassportPhotoName));
                        cmd.Parameters.AddWithValue("@DrivingLicensePhotoName", CheckForDbNull(model.DrivingLicensePhotoName));

                        cmd.Parameters.AddWithValue("@Year", model.Year);
                        cmd.Parameters.AddWithValue("@Condition", model.Condition);
                        cmd.Parameters.AddWithValue("@TruckFrontPhotoName", CheckForDbNull(model.TruckFrontPhotoName));
                        cmd.Parameters.AddWithValue("@TruckBackPhotoName", CheckForDbNull(model.TruckBackPhotoName));
                        cmd.Parameters.AddWithValue("@TruckRightPhotoName", CheckForDbNull(model.TruckRightPhotoName));
                        cmd.Parameters.AddWithValue("@TruckLeftPhotoName", CheckForDbNull(model.TruckLeftPhotoName));

                        cmd.Parameters.AddWithValue("@TransportTypeId", model.TransportTypeId);
                        cmd.Parameters.AddWithValue("@CompanyId", model.CompanyId);

                        cmd.Parameters.AddWithValue("@IsDeleted", false);
                        cmd.Parameters.AddWithValue("@CreateDate", GetDateTimeNow());
                        //cmd.ExecuteNonQuery();
                        insertedId = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                        if (insertedId <= 0) throw new Exception("Supplier does not saved");

                        string nameFront = null;
                        string nameBack = null;
                        string nameRight = null;
                        string nameLeft = null;

                        string namePass = null;
                        string namePass2 = null;
                        string nameInter = null;
                        string nameLic = null;

                        if (front != null || back != null || right != null || left != null)
                        {
                            if (front != null)
                            {
                                nameFront = MediaHelper.SaveImageCreate(front, insertedId.ToString() + "_f", "Supplier");
                                if (string.IsNullOrEmpty(nameFront))
                                    throw new Exception("Error on image save. (front)");
                            }

                            if (back != null)
                            {
                                nameBack = MediaHelper.SaveImageCreate(back, insertedId.ToString() + "_b", "Supplier");
                                if (string.IsNullOrEmpty(nameBack))
                                    throw new Exception("Error on image save. (back)");
                            }

                            if (right != null)
                            {
                                nameRight = MediaHelper.SaveImageCreate(right, insertedId.ToString() + "_r", "Supplier");
                                if (string.IsNullOrEmpty(nameRight))
                                    throw new Exception("Error on image save. (right)");
                            }

                            if (left != null)
                            {
                                nameLeft = MediaHelper.SaveImageCreate(left, insertedId.ToString() + "_l", "Supplier");
                                if (string.IsNullOrEmpty(nameLeft))
                                    throw new Exception("Error on image save. (left)");
                            }

                            if (pass != null)
                            {
                                namePass = MediaHelper.SaveImageCreate(pass, insertedId.ToString() + "_p", "Supplier");
                                if (string.IsNullOrEmpty(namePass))
                                    throw new Exception("Error on image save. (passport)");
                            }

                            if (pass2 != null)
                            {
                                namePass2 = MediaHelper.SaveImageCreate(pass2, insertedId.ToString() + "_p2", "Supplier");
                                if (string.IsNullOrEmpty(namePass2))
                                    throw new Exception("Error on image save. (passport2)");
                            }

                            if (inter != null)
                            {
                                nameInter = MediaHelper.SaveImageCreate(inter, insertedId.ToString() + "_i", "Supplier");
                                if (string.IsNullOrEmpty(nameInter))
                                    throw new Exception("Error on image save. (inter pass)");
                            }

                            if (lic != null)
                            {
                                nameLic = MediaHelper.SaveImageCreate(lic, insertedId.ToString() + "_d", "Supplier");
                                if (string.IsNullOrEmpty(nameLic))
                                    throw new Exception("Error on image save. (license)");
                            }

                            updateSupplierImages(insertedId, nameFront, nameBack, nameRight, nameLeft, namePass, namePass2, nameInter, nameLic);
                        }
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

        public bool EditSupplier(Supplier model, HttpPostedFileBase inter, HttpPostedFileBase lic, HttpPostedFileBase pass, HttpPostedFileBase pass2,
                                                 HttpPostedFileBase front, HttpPostedFileBase back, HttpPostedFileBase right, HttpPostedFileBase left,
                                string username, string password)
        {
            ValidateAuthorization(username, password);
            Validator.SupplierEditValidation(model, inter, lic, pass, pass2, front, back, right, left);

            try
            {
                OpenConnection();

                const string sql = @"UPDATE Suppliers SET 	  FirstName = @FirstName
                                                            , LastName = @LastName
                                                            , WhatsappNumber = @WhatsappNumber
                                                            , ContactNumber = @ContactNumber
                                                            , PlateNumber = @PlateNumber
                                                            , PlateNumber2 = @PlateNumber2
                                                            , TransportPassport = @TransportPassport
                                                            , TransportPassport2 = @TransportPassport2
                                                            , InternationalPassport = @InternationalPassport 
                                                            , DrivingLicense = @DrivingLicense
                                                            , Year = @Year
                                                            , Condition = @Condition
                                                          --  , TruckFrontPhotoName = @TruckFrontPhotoName
                                                          --  , TruckBackPhotoName = @TruckBackPhotoName
                                                          --  , TruckRightPhotoName = @TruckRightPhotoName
                                                          --  , TruckLeftPhotoName = @TruckLeftPhotoName
                                                            , TransportTypeId = @TransportTypeId
                                                            , CompanyId = @CompanyId 
                                                              WHERE Id = @Id AND IsDeleted = @IsDeleted";

                using (var cmd = new SqlCommand(sql, _conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Id", model.Id);

                    cmd.Parameters.AddWithValue("@FirstName", model.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", model.LastName);

                    cmd.Parameters.AddWithValue("@WhatsappNumber", CheckForDbNull(model.WhatsappNumber));
                    cmd.Parameters.AddWithValue("@ContactNumber", CheckForDbNull(model.ContactNumber));
                    cmd.Parameters.AddWithValue("@PlateNumber", CheckForDbNull(model.PlateNumber));
                    cmd.Parameters.AddWithValue("@PlateNumber2", CheckForDbNull(model.PlateNumber2));
                    cmd.Parameters.AddWithValue("@TransportPassport", CheckForDbNull(model.TransportPassport));
                    cmd.Parameters.AddWithValue("@TransportPassport2", CheckForDbNull(model.TransportPassport2));
                    cmd.Parameters.AddWithValue("@InternationalPassport", CheckForDbNull(model.InternationalPassport));
                    cmd.Parameters.AddWithValue("@DrivingLicense", CheckForDbNull(model.DrivingLicense));
                    cmd.Parameters.AddWithValue("@Year", model.Year);
                    cmd.Parameters.AddWithValue("@Condition", model.Condition);

                    //cmd.Parameters.AddWithValue("@TruckFrontPhotoName", CheckForDbNull(model.TruckFrontPhotoName));
                    //cmd.Parameters.AddWithValue("@TruckBackPhotoName", CheckForDbNull(model.TruckBackPhotoName));
                    //cmd.Parameters.AddWithValue("@TruckRightPhotoName", CheckForDbNull(model.TruckRightPhotoName));
                    //cmd.Parameters.AddWithValue("@TruckLeftPhotoName", CheckForDbNull(model.TruckLeftPhotoName));

                    cmd.Parameters.AddWithValue("@TransportTypeId", model.TransportTypeId);
                    cmd.Parameters.AddWithValue("@CompanyId", model.CompanyId);

                    cmd.Parameters.AddWithValue("@IsDeleted", false);
                    cmd.ExecuteNonQuery();
                }


                string nameFront = null;
                string nameBack = null;
                string nameRight = null;
                string nameLeft = null;
                string namePass = null;
                string namePass2 = null;
                string nameInter = null;
                string nameLic = null;

                if (front != null || back != null || right != null || left != null)
                {
                    if (front != null)
                    {
                        if (string.IsNullOrEmpty(model.TruckFrontPhotoName)) //create
                        {
                            nameFront = MediaHelper.SaveImageCreate(front, model.Id.ToString() + "_f", "Supplier");
                        }
                        else  //edit
                        {
                            nameFront = MediaHelper.SaveImageEdit(front, model.Id.ToString() + "_f", model.TruckFrontPhotoName, "Supplier");
                        }

                        if (string.IsNullOrEmpty(nameFront))
                            throw new Exception("Error on image save. (front)");
                    }
                    else
                        nameFront = model.TruckFrontPhotoName;

                    if (back != null)
                    {
                        if (string.IsNullOrEmpty(model.TruckBackPhotoName)) //create
                        {
                            nameBack = MediaHelper.SaveImageCreate(back, model.Id.ToString() + "_b", "Supplier");
                        }
                        else  //edit
                        {
                            nameBack = MediaHelper.SaveImageEdit(back, model.Id.ToString() + "_b", model.TruckBackPhotoName, "Supplier");
                        }

                        if (string.IsNullOrEmpty(nameBack))
                            throw new Exception("Error on image save. (back)");
                    }
                    else
                        nameBack = model.TruckBackPhotoName;

                    if (right != null)
                    {
                        if (string.IsNullOrEmpty(model.TruckRightPhotoName)) //create
                        {
                            nameRight = MediaHelper.SaveImageCreate(right, model.Id.ToString() + "_r", "Supplier");
                        }
                        else  //edit
                        {
                            nameRight = MediaHelper.SaveImageEdit(right, model.Id.ToString() + "_r", model.TruckRightPhotoName, "Supplier");
                        }

                        if (string.IsNullOrEmpty(nameRight))
                            throw new Exception("Error on image save. (right)");
                    }
                    else
                        nameRight = model.TruckRightPhotoName;

                    if (left != null)
                    {
                        if (string.IsNullOrEmpty(model.TruckLeftPhotoName)) //create
                        {
                            nameLeft = MediaHelper.SaveImageCreate(left, model.Id.ToString() + "_l", "Supplier");
                        }
                        else  //edit
                        {
                            nameLeft = MediaHelper.SaveImageEdit(left, model.Id.ToString() + "_l", model.TruckLeftPhotoName, "Supplier");
                        }

                        if (string.IsNullOrEmpty(nameLeft))
                            throw new Exception("Error on image save. (left)");
                    }
                    else
                        nameLeft = model.TruckLeftPhotoName;


                    if (pass != null)
                    {
                        if (string.IsNullOrEmpty(model.TransportPassportPhotoName)) //create
                        {
                            namePass = MediaHelper.SaveImageCreate(pass, model.Id.ToString() + "_p", "Supplier");
                        }
                        else  //edit
                        {
                            namePass = MediaHelper.SaveImageEdit(pass, model.Id.ToString() + "_p", model.TransportPassportPhotoName, "Supplier");
                        }

                        if (string.IsNullOrEmpty(namePass))
                            throw new Exception("Error on image save. (Trans. Pass.)");
                    }
                    else
                        namePass = model.TransportPassportPhotoName;


                    if (pass2 != null)
                    {
                        if (string.IsNullOrEmpty(model.TransportPassport2PhotoName)) //create
                        {
                            namePass2 = MediaHelper.SaveImageCreate(pass2, model.Id.ToString() + "_p2", "Supplier");
                        }
                        else  //edit
                        {
                            namePass2 = MediaHelper.SaveImageEdit(pass2, model.Id.ToString() + "_p2", model.TransportPassport2PhotoName, "Supplier");
                        }

                        if (string.IsNullOrEmpty(namePass2))
                            throw new Exception("Error on image save. (Trans. Pass 2.)");
                    }
                    else
                        namePass2 = model.TransportPassport2PhotoName;


                    if (inter != null)
                    {
                        if (string.IsNullOrEmpty(model.InternationalPassportPhotoName)) //create
                        {
                            nameInter = MediaHelper.SaveImageCreate(inter, model.Id.ToString() + "_i", "Supplier");
                        }
                        else  //edit
                        {
                            nameInter = MediaHelper.SaveImageEdit(inter, model.Id.ToString() + "_i", model.InternationalPassportPhotoName, "Supplier");
                        }

                        if (string.IsNullOrEmpty(nameInter))
                            throw new Exception("Error on image save. (Inter .)");
                    }
                    else
                        nameInter = model.InternationalPassportPhotoName;


                    if (lic != null)
                    {
                        if (string.IsNullOrEmpty(model.DrivingLicensePhotoName)) //create
                        {
                            nameLic = MediaHelper.SaveImageCreate(lic, model.Id.ToString() + "_i", "Supplier");
                        }
                        else  //edit
                        {
                            nameLic = MediaHelper.SaveImageEdit(lic, model.Id.ToString() + "_i", model.DrivingLicensePhotoName, "Supplier");
                        }

                        if (string.IsNullOrEmpty(nameLic))
                            throw new Exception("Error on image save. (Driver Lic.)");
                    }
                    else
                        nameLic = model.DrivingLicensePhotoName;


                    updateSupplierImages(model.Id, nameFront, nameBack, nameRight, nameLeft, namePass, namePass2, nameInter, nameLic);
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

        public bool DeleteSupplier(int id, string username, string password)
        {
            using (var t = new TransactionScope())
            {
                try
                {
                    OpenConnection();

                    var model = getSupplierById(id);
                    if (model == null) throw new Exception("Supplier not found");

                    string nameFront = model.TruckFrontPhotoName;
                    string nameBack = model.TruckBackPhotoName;
                    string nameRight = model.TruckRightPhotoName;
                    string nameLeft = model.TruckLeftPhotoName;

                    string namePass = model.TransportPassportPhotoName;
                    string namePass2 = model.TransportPassport2PhotoName;
                    string nameInter = model.InternationalPassportPhotoName;
                    string nameLic = model.DrivingLicensePhotoName;


                    const string sql = "UPDATE Suppliers SET IsDeleted = @IsDeleted WHERE Id = @Id";
                    using (var cmd = new SqlCommand(sql, _conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@IsDeleted", true);
                        cmd.ExecuteNonQuery();
                    }

                    //delete image
                    MediaHelper.DeleteImageFromFolder(nameFront, "Supplier");
                    MediaHelper.DeleteImageFromFolder(nameBack, "Supplier");
                    MediaHelper.DeleteImageFromFolder(nameRight, "Supplier");
                    MediaHelper.DeleteImageFromFolder(nameLeft, "Supplier");

                    MediaHelper.DeleteImageFromFolder(namePass, "Supplier");
                    MediaHelper.DeleteImageFromFolder(namePass2, "Supplier");
                    MediaHelper.DeleteImageFromFolder(nameInter, "Supplier");
                    MediaHelper.DeleteImageFromFolder(nameLic, "Supplier");

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


        //get methods
        public Supplier GetSupplierById(int id, string username, string password)
        {
            ValidateAuthorization(username, password);

            try
            {
                OpenConnection();

                return getSupplierById(id);
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

        public SupplierDataModel GetSupplierDataModel(int id, string username, string password)
        {
            ValidateAuthorization(username, password);

            try
            {
                OpenConnection();
                SupplierDataModel model = new SupplierDataModel();
                model.Supplier = getSupplierById(id);

                if (model.Supplier == null)
                    throw new Exception("Supplier not found");

                model.Company = getCompanyById(model.Supplier.CompanyId);
                model.TransportType = getTransportTypeById(model.Supplier.TransportTypeId);

                CloseConnection();
                return model;
            }
            catch (Exception)
            {
                CloseConnection();
                throw;
            }
            finally
            {

            }
        }

        public SupplierEntityCollection LoadSuppliersByCriteria(SupplierCriteriaModel criteria, int rowsPerPage, int pageNumber, string username, string password)
        {
            ValidateAuthorization(username, password);

            try
            {
                OpenConnection();

                SupplierEntityCollection result = new SupplierEntityCollection();
                result.Suppliers = new List<Supplier>();

                string _minCreateDate = (criteria.MinCreateDate == null || criteria.MinCreateDate.Value == DateTime.MinValue) ? string.Empty : " AND CreateDate >= @MinCreateDate ";
                string _maxCreateDate = (criteria.MaxCreateDate == null || criteria.MaxCreateDate.Value == DateTime.MinValue) ? string.Empty : " AND CreateDate <= @MaxCreateDate ";

                //calculate count
                string sqlCount = "SELECT COUNT(*) FROM Suppliers WHERE IsDeleted = @IsDeleted " + _minCreateDate + _maxCreateDate;
                using (var cmdCount = new SqlCommand(sqlCount, _conn))
                {
                    cmdCount.CommandType = CommandType.Text;
                    if (!string.IsNullOrEmpty(_minCreateDate)) { cmdCount.Parameters.AddWithValue("@MinCreateDate", criteria.MinCreateDate); }
                    if (!string.IsNullOrEmpty(_maxCreateDate)) { cmdCount.Parameters.AddWithValue("@MaxCreateDate", criteria.MaxCreateDate); }
                    cmdCount.Parameters.AddWithValue("@IsDeleted", false);
                    result.AllCount = Convert.ToInt32(cmdCount.ExecuteScalar());
                }

                //calculate list
                string sql = @"SELECT Id
                                    , FirstName 		
                                    , LastName
                                    , WhatsappNumber
                                    , ContactNumber
                                    , PlateNumber
                                    , PlateNumber2
                                    , TransportPassport
                                    , TransportPassport2
                                    , InternationalPassport
                                    , DrivingLicense
                                    , TransportPassportPhotoName
                                    , TransportPassport2PhotoName
                                    , InternationalPassportPhotoName
                                    , DrivingLicensePhotoName
                                    , Year
                                    , Condition
                                    , TruckFrontPhotoName
                                    , TruckBackPhotoName
                                    , TruckRightPhotoName
                                    , TruckLeftPhotoName
                                    , TransportTypeId
                                    , CompanyId, IsDeleted, CreateDate FROM ( " +

                             @"SELECT Id
                                    , FirstName 		
                                    , LastName
                                    , WhatsappNumber
                                    , ContactNumber
                                    , PlateNumber
                                    , PlateNumber2
                                    , TransportPassport
                                    , TransportPassport2
                                    , InternationalPassport
                                    , DrivingLicense
                                    , TransportPassportPhotoName
                                    , TransportPassport2PhotoName
                                    , InternationalPassportPhotoName
                                    , DrivingLicensePhotoName
                                    , Year
                                    , Condition
                                    , TruckFrontPhotoName
                                    , TruckBackPhotoName
                                    , TruckRightPhotoName
                                    , TruckLeftPhotoName
                                    , TransportTypeId
                                    , CompanyId, IsDeleted, CreateDate, " +
                             "ROW_NUMBER() OVER (ORDER BY CreateDate DESC) AS RowNum FROM Suppliers WHERE IsDeleted = @IsDeleted " +
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
                            result.Suppliers = readSupplierListFromDataReader(dr);

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

        //public List<Supplier> LoadSuppliers(string username, string password)
        //{
        //    ValidateAuthorization(username, password);

        //    try
        //    {
        //        OpenConnection();
        //        List<Supplier> result = loadSuppliers();
        //        CloseConnection();
        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        CloseConnection();
        //        throw;
        //    }
        //}


        //DB repo methods
        //private List<Supplier> loadSuppliers()
        //{
        //    List<Supplier> result = new List<Supplier>();

        //    string sql = "SELECT * FROM Suppliers WHERE IsDeleted = @IsDeleted ";
        //    using (var cmd = new SqlCommand(sql, _conn))
        //    {
        //        cmd.CommandType = CommandType.Text;
        //        cmd.Parameters.AddWithValue("@IsDeleted", false);
        //        using (var dr = cmd.ExecuteReader())
        //        {
        //            if (dr.HasRows)
        //            {
        //                result = readSupplierListFromDataReader(dr);
        //                return result;
        //            }
        //        }
        //    }
        //    return result;
        //}
        private Supplier getSupplierById(int id)
        {
            const string sql = "SELECT TOP 1 * FROM Suppliers WHERE Id = @Id AND IsDeleted = @IsDeleted";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@IsDeleted", false);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                        return readSupplierFromDataReader(dr);
                    else
                        return null;
                }
            }
        }
        private bool updateSupplierImages(int id, string nameFront, string nameBack, string nameRight, string nameLeft,
                                                  string namePass, string namePass2, string nameInter, string nameLic)
        {
            const string sql = "UPDATE Suppliers SET TruckFrontPhotoName = @TruckFrontPhotoName, TruckBackPhotoName = @TruckBackPhotoName, TruckRightPhotoName = @TruckRightPhotoName, " +
                               "TruckLeftPhotoName = @TruckLeftPhotoName, TransportPassportPhotoName = @TransportPassportPhotoName, TransportPassport2PhotoName = @TransportPassport2PhotoName, " +
                               "InternationalPassportPhotoName = @InternationalPassportPhotoName, DrivingLicensePhotoName = @DrivingLicensePhotoName  WHERE Id = @Id AND IsDeleted = @IsDeleted";

            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@TruckFrontPhotoName", CheckForDbNull(nameFront));
                cmd.Parameters.AddWithValue("@TruckBackPhotoName", CheckForDbNull(nameBack));
                cmd.Parameters.AddWithValue("@TruckRightPhotoName", CheckForDbNull(nameRight));
                cmd.Parameters.AddWithValue("@TruckLeftPhotoName", CheckForDbNull(nameLeft));

                cmd.Parameters.AddWithValue("@TransportPassportPhotoName", CheckForDbNull(namePass));
                cmd.Parameters.AddWithValue("@TransportPassport2PhotoName", CheckForDbNull(namePass2));
                cmd.Parameters.AddWithValue("@InternationalPassportPhotoName", CheckForDbNull(nameInter));
                cmd.Parameters.AddWithValue("@DrivingLicensePhotoName", CheckForDbNull(nameLic));

                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@IsDeleted", false);
                cmd.ExecuteNonQuery();
                return true;
            }
        }
        private bool checkSupplierExistBy(string by, int id)
        {
            string sql = "SELECT TOP 1 * FROM Suppliers WHERE " + by + " = @" + by + " AND IsDeleted = @IsDeleted";
            using (var cmd = new SqlCommand(sql, _conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@" + by, id);
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
        private Supplier readSupplierFromDataReader(SqlDataReader dr)
        {
            Supplier model = null;
            while (dr.Read())
            {
                model = new Supplier();
                model.Id = Convert.ToInt32(dr["Id"].ToString());

                model.FirstName = dr["FirstName"].ToString();
                model.LastName = dr["LastName"].ToString();
                model.WhatsappNumber = dr["WhatsappNumber"].ToString();
                model.ContactNumber = dr["ContactNumber"].ToString();
                model.PlateNumber = dr["PlateNumber"].ToString();
                model.PlateNumber2 = dr["PlateNumber2"].ToString();
                model.TransportPassport = dr["TransportPassport"].ToString();
                model.TransportPassport2 = dr["TransportPassport2"].ToString();
                model.DrivingLicense = dr["DrivingLicense"].ToString();

                model.TransportPassportPhotoName = dr["TransportPassportPhotoName"].ToString();
                model.TransportPassport2PhotoName = dr["TransportPassport2PhotoName"].ToString();
                model.InternationalPassportPhotoName = dr["InternationalPassportPhotoName"].ToString();
                model.DrivingLicensePhotoName = dr["DrivingLicensePhotoName"].ToString();

                model.Year = Convert.ToInt32(dr["Year"].ToString());
                model.Condition = Convert.ToInt32(dr["Condition"].ToString());
                model.TruckFrontPhotoName = dr["TruckFrontPhotoName"].ToString();
                model.TruckBackPhotoName = dr["TruckBackPhotoName"].ToString();
                model.TruckRightPhotoName = dr["TruckRightPhotoName"].ToString();
                model.TruckLeftPhotoName = dr["TruckLeftPhotoName"].ToString();
                model.TransportTypeId = Convert.ToInt32(dr["TransportTypeId"].ToString());
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString());

                model.IsDeleted = Convert.ToBoolean(dr["IsDeleted"].ToString());
                model.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());
            }
            return model;
        }
        private List<Supplier> readSupplierListFromDataReader(SqlDataReader dr)
        {
            List<Supplier> result = new List<Supplier>();
            while (dr.Read())
            {
                Supplier model = new Supplier();
                model.Id = Convert.ToInt32(dr["Id"].ToString());

                model.FirstName = dr["FirstName"].ToString();
                model.LastName = dr["LastName"].ToString();
                model.WhatsappNumber = dr["WhatsappNumber"].ToString();
                model.ContactNumber = dr["ContactNumber"].ToString();
                model.PlateNumber = dr["PlateNumber"].ToString();
                model.PlateNumber2 = dr["PlateNumber2"].ToString();
                model.TransportPassport = dr["TransportPassport"].ToString();
                model.TransportPassport2 = dr["TransportPassport2"].ToString();
                model.DrivingLicense = dr["DrivingLicense"].ToString();

                model.TransportPassportPhotoName = dr["TransportPassportPhotoName"].ToString();
                model.TransportPassport2PhotoName = dr["TransportPassport2PhotoName"].ToString();
                model.InternationalPassportPhotoName = dr["InternationalPassportPhotoName"].ToString();
                model.DrivingLicensePhotoName = dr["DrivingLicensePhotoName"].ToString();

                model.Year = Convert.ToInt32(dr["Year"].ToString());
                model.Condition = Convert.ToInt32(dr["Condition"].ToString());
                model.TruckFrontPhotoName = dr["TruckFrontPhotoName"].ToString();
                model.TruckBackPhotoName = dr["TruckBackPhotoName"].ToString();
                model.TruckRightPhotoName = dr["TruckRightPhotoName"].ToString();
                model.TruckLeftPhotoName = dr["TruckLeftPhotoName"].ToString();
                model.TransportTypeId = Convert.ToInt32(dr["TransportTypeId"].ToString());
                model.CompanyId = Convert.ToInt32(dr["CompanyId"].ToString());

                model.IsDeleted = Convert.ToBoolean(dr["IsDeleted"].ToString());
                model.CreateDate = DateTime.Parse(dr["CreateDate"].ToString());

                result.Add(model);
            }
            return result;
        }
    }
}

