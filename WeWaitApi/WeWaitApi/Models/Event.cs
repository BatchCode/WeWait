using System;
using System.Collections.Generic;

namespace WeWaitApi.Models
{
    public partial class Event
    {
        public Event()
        {
            Eventbooking = new HashSet<Eventbooking>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Actor { get; set; }
        public int? Seats { get; set; }
        public DateTime? DateStart { get; set; }
        public string DateEnd { get; set; }
        public int? LocationId { get; set; }

        public virtual Location Location { get; set; }
        public virtual ICollection<Eventbooking> Eventbooking { get; set; }
    }
}
