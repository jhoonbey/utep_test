using app.domain.Model.Entities;
using System;
using System.DataManager;

namespace app.database.Validations
{
    public partial class Validator
    {
        public static void TransportTypeCreateValidation(TransportType model)
        {
            //model
            if (model == null) throw new Exception("Fill form correct");

            if (string.IsNullOrEmpty(model.Name) || Common.IsOnlySpace(model.Name)) throw new Exception("Name is empty");
        }

        public static void TransportTypeEditValidation(TransportType model)
        {
            //model
            if (model == null) throw new Exception("Fill form correct");

            if (string.IsNullOrEmpty(model.Name) || Common.IsOnlySpace(model.Name)) throw new Exception("Name is empty");
        }
    }
}
