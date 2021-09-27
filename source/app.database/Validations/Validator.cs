//using JhoonHelper;
using System;
using System.DataManager;
using System.Text.RegularExpressions;
using System.Web;

namespace app.database.Validations
{
    public partial class Validator
    {
        public static void ImageValidation(HttpPostedFileBase postedFile, int minHeight, int maxHeight, int minWidth, int maxWidth, bool watchSizes = false)
        {
            //format
            string[] allowedExtensions = new string[] { ".JPEG", ".JPG", ".PNG" };
            if (!MediaHelper.IsValidFile(postedFile.FileName, allowedExtensions)) throw new Exception("Only select photos in .JPEG, .JPG and .PNG formats");

            //length - 1 kb = 1024,       1 mb = 1048576  byte,   50 mb = 52428800
            if (!MediaHelper.IsValidLength(postedFile.ContentLength, 100, 10485760)) throw new Exception("Select photo in minimum 100 byte, maximum 10 MB");

            //Dimensions
            if (watchSizes)
            {
                if (!MediaHelper.IsValidImageByDimensions(postedFile, minHeight, maxHeight, minWidth, maxWidth))
                {
                    throw new Exception("Select photo in " + minWidth.ToString() + "px" + " width and " + minHeight.ToString() + "px" + " height");
                }
            }

        }

        //public static void ContactEmailSendValidation(ContactEmailModel model)
        //{
        //    //model
        //    if (model == null) throw new Exception("Fill the form correctly");

        //    //name
        //    if (string.IsNullOrEmpty(model.Name) || model.Name.Length < 3 || model.Name.Length > 50) throw new Exception("Name is not correct. Use minimum 3, maximum 50 characters");

        //    //mail
        //    Regex rxEmail = new Regex(@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        //                             + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
        //                            [0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        //                             + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
        //                            [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        //                             + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$");

        //    //message
        //    if (string.IsNullOrEmpty(model.Message) || model.Message.Length < 10 || model.Message.Length > 1000 || Common.IsOnlySpace(model.Message))
        //        throw new Exception("Message is not correct. Use minimum 10, maximum 1000 characters");
        //}
    }
}
