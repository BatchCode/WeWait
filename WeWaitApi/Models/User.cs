using System;
using System.Collections.Generic;

namespace WeWaitApi.Models
{
    public partial class User
    {
        public User()
        {
            BookingUser = new HashSet<Booking>();
            BookingWeWaiter = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompteBancaire { get; set; }
        public int? RoleId { get; set; }
        public int? LocationId { get; set; }

        public virtual Location Location { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Booking> BookingUser { get; set; }
        public virtual ICollection<Booking> BookingWeWaiter { get; set; }
    }
}
