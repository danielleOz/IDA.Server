using System;
using System.Collections.Generic;

#nullable disable

namespace IDA.ServerBL.Models
{
    public partial class Status
    {
        public Status()
        {
            JobOffers = new HashSet<JobOffer>();
        }

        public int StatusId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<JobOffer> JobOffers { get; set; }
    }
}
