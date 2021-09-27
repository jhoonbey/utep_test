using app.domain.Model.Entities;
using System;
using System.DataManager;

namespace app.database.Validations
{
    public partial class Validator
    {
        public static void CityCreateValidation(City model)
        {
            //model
            if (model == null) throw new Exception("Fill form correct");


            //name
            if (string.IsNullOrEmpty(model.Name) || Common.IsOnlySpace(model.Name)) throw new Exception("Name is empty");


            //CountryId
            if (model.CountryId <= 0) throw new Exception("Incorrect Country");
        }

        public static void CityEditValidation(City model)
        {
            //model
            if (model == null) throw new Exception("Fill form correct");

            //name
            if (string.IsNullOrEmpty(model.Name) || Common.IsOnlySpace(model.Name)) throw new Exception("Name is empty");


            //CountryId
            if (model.CountryId <= 0) throw new Exception("Incorrect Country");
        }
    }
}
