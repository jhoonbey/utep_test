using app.domain.Model.Entities;
using System.Collections.Generic;

namespace app.domain.Model.EntityCollections
{
    public class SupplierEntityCollection : BaseEntityCollection
    {
        public List<Supplier> Suppliers { get; set; }
    }
}
