using System;
using System.Collections.Generic;

namespace WeWaitApi.Models
{
    public partial class Confirmbooking
    {
        public Confirmbooking()
        {
            Booking = new HashSet<Booking>();
        }

        public int Id { get; set; }
        public string BookingCode { get; set; }
        public sbyte Confirm { get; set; }

        public virtual ICollection<Booking> Booking { get; set; }
    }
}
