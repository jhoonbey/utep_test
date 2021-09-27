using app.domain.Model.Entities;
using System;
using System.DataManager;
using System.Web;

namespace app.database.Validations
{
    public partial class Validator
    {
        public static void SupplierCreateValidation(Supplier model, HttpPostedFileBase inter, HttpPostedFileBase lic, HttpPostedFileBase pass, HttpPostedFileBase pass2,
                                                                    HttpPostedFileBase front, HttpPostedFileBase back, HttpPostedFileBase right, HttpPostedFileBase left)
        {
            //model
            if (model == null) throw new Exception("Fill form correct");

            if (string.IsNullOrEmpty(model.FirstName) || Common.IsOnlySpace(model.FirstName)) throw new Exception("First name is empty");

            if (string.IsNullOrEmpty(model.LastName) || Common.IsOnlySpace(model.LastName)) throw new Exception("Last name is empty");



            if (inter != null)
            {
                ImageValidation(inter, 0, 0, 0, 0, false);
            }

            if (lic != null)
            {
                ImageValidation(lic, 0, 0, 0, 0, false);
            }

            if (pass != null)
            {
                ImageValidation(pass, 0, 0, 0, 0, false);
            }

            if (pass2 != null)
            {
                ImageValidation(pass2, 0, 0, 0, 0, false);
            }

            if (front != null)
            {
                ImageValidation(front, 0, 0, 0, 0, false);
            }

            if (back != null)
            {
                ImageValidation(back, 0, 0, 0, 0, false);
            }

            if (right != null)
            {
                ImageValidation(right, 0, 0, 0, 0, false);
            }

            if (left != null)
            {
                ImageValidation(left, 0, 0, 0, 0, false);
            }
        }


        public static void SupplierEditValidation(Supplier model, HttpPostedFileBase inter, HttpPostedFileBase lic, HttpPostedFileBase pass, HttpPostedFileBase pass2,
                                                                  HttpPostedFileBase front, HttpPostedFileBase back, HttpPostedFileBase right, HttpPostedFileBase left)
        {
            //model
            if (model == null) throw new Exception("Fill form correct");

            if (string.IsNullOrEmpty(model.FirstName) || Common.IsOnlySpace(model.FirstName)) throw new Exception("First name is empty");

            if (string.IsNullOrEmpty(model.LastName) || Common.IsOnlySpace(model.LastName)) throw new Exception("Last name is empty");


            if (inter != null)
            {
                ImageValidation(inter, 0, 0, 0, 0, false);
            }

            if (lic != null)
            {
                ImageValidation(lic, 0, 0, 0, 0, false);
            }

            if (pass != null)
            {
                ImageValidation(pass, 0, 0, 0, 0, false);
            }

            if (pass2 != null)
            {
                ImageValidation(pass2, 0, 0, 0, 0, false);
            }

            if (front != null)
            {
                ImageValidation(front, 0, 0, 0, 0, false);
            }

            if (back != null)
            {
                ImageValidation(back, 0, 0, 0, 0, false);
            }

            if (right != null)
            {
                ImageValidation(right, 0, 0, 0, 0, false);
            }

            if (left != null)
            {
                ImageValidation(left, 0, 0, 0, 0, false);
            }
        }
    }
}
