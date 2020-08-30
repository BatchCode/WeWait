using System;
using System.Collections.Generic;

namespace WeWaitApi.Models
{
    public partial class Location
    {
        public Location()
        {
            Event = new HashSet<Event>();
            User = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Adress1 { get; set; }
        public string Adress2 { get; set; }
        public string PostalCode { get; set; }

        public virtual ICollection<Event> Event { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
