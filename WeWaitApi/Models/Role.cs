using System;
using System.Collections.Generic;

namespace WeWaitApi.Models
{
    public partial class Role
    {
        public Role()
        {
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Label { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
