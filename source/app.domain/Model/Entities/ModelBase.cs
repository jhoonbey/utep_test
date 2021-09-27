using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace app.domain.Model.Entities
{
    public class ModelBase
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
