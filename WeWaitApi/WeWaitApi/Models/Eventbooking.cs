using System;
using System.Collections.Generic;

namespace WeWaitApi.Models
{
    public partial class Eventbooking
    {
        public int EventId { get; set; }
        public int BookingId { get; set; }
        public int Quantity { get; set; }

        public virtual Booking Booking { get; set; }
        public virtual Event Event { get; set; }
    }
}
