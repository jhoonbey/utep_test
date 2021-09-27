using app.domain.Model.Entities;
using System.Collections.Generic;

namespace app.domain.Model.EntityCollections
{
    public class CityEntityCollection : BaseEntityCollection
    {
        public List<City> Cities { get; set; }
    }
}
