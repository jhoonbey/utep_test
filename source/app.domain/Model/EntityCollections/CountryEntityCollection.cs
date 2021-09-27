using app.domain.Model.Entities;
using System.Collections.Generic;

namespace app.domain.Model.EntityCollections
{
    public class CountryEntityCollection : BaseEntityCollection
    {
        public List<Country> Countries { get; set; }
    }
}
