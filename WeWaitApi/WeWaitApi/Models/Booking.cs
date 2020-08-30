using System;
using System.Collections.Generic;

namespace WeWaitApi.Models
{
    public partial class Booking
    {
        public Booking()
        {
            Eventbooking = new HashSet<Eventbooking>();
            Satehistory = new HashSet<Satehistory>();
        }

        public int Id { get; set; }
        public DateTime? Time { get; set; }
        public DateTime? TimeCancel { get; set; }
        public string Code { get; set; }
        public sbyte? Confirm { get; set; }
        public int? UserId { get; set; }
        public int? WeWaiterId { get; set; }

        public virtual User User { get; set; }
        public virtual User WeWaiter { get; set; }
        public virtual ICollection<Eventbooking> Eventbooking { get; set; }
        public virtual ICollection<Satehistory> Satehistory { get; set; }
    }
}
