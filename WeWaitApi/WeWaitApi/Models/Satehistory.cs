using System;
using System.Collections.Generic;

namespace WeWaitApi.Models
{
    public partial class Satehistory
    {
        public int StateId { get; set; }
        public int BookingId { get; set; }

        public virtual Booking Booking { get; set; }
        public virtual State State { get; set; }
    }
}
