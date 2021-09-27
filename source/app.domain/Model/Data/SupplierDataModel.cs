using app.domain.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.domain.Model.Data
{
    public class SupplierDataModel
    {
        public Supplier Supplier { get; set; }
        public TransportType TransportType { get; set; }
        public Company Company { get; set; }
    }
}
