using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace app.domain.Model.Entities
{
    public class User : ModelBase
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public int Role { get; set; }

        public string Fullname { get; set; }
    }
}
