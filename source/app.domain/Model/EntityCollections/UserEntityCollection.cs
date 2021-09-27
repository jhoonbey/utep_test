using app.domain.Model.Entities;
using System.Collections.Generic;

namespace app.domain.Model.EntityCollections
{
    public class UserEntityCollection : BaseEntityCollection
    {
        public List<User> Users { get; set; }
    }
}
