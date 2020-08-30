using System;
using System.Collections.Generic;

namespace WeWaitApi.Models
{
    public partial class State
    {
        public State()
        {
            Satehistory = new HashSet<Satehistory>();
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Satehistory> Satehistory { get; set; }
    }
}
