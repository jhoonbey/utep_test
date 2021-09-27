using app.domain.Model.Entities;
using System.Collections.Generic;

namespace app.domain.Model.EntityCollections
{
    public class CompanyEntityCollection : BaseEntityCollection
    {
        public List<Company> Companies { get; set; }
    }
}
