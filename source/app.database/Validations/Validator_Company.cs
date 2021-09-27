using app.domain.Model.Entities;
using System;
using System.DataManager;

namespace app.database.Validations
{
    public partial class Validator
    {
        public static void CompanyCreateValidation(Company model)
        {
            //model
            if (model == null) throw new Exception("Fill form correct");

            if (string.IsNullOrEmpty(model.Name) || Common.IsOnlySpace(model.Name)) throw new Exception("Name is empty");
        }

        public static void CompanyEditValidation(Company model)
        {
            //model
            if (model == null) throw new Exception("Fill form correct");

            if (string.IsNullOrEmpty(model.Name) || Common.IsOnlySpace(model.Name)) throw new Exception("Name is empty");
        }
    }
}
