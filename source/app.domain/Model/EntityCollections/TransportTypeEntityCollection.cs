using app.domain.Model.Entities;
using System.Collections.Generic;

namespace app.domain.Model.EntityCollections
{
    public class TransportTypeEntityCollection : BaseEntityCollection
    {
        public List<TransportType> TransportTypes { get; set; }
    }
}
