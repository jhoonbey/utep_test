using app.database;
using System.ServiceModel.Activation;

namespace app.service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class UtepService : IUtepService
    {
        private AppDatabase DataBase;
        public UtepService()
        {
            DataBase = new AppDatabase();
        }
    }
}
