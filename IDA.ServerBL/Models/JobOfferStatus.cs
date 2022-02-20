using System;
using System.Collections.Generic;

#nullable disable

namespace IDA.ServerBL.Models
{
    public partial class JobOfferStatus
    {
        public JobOfferStatus()
        {
            JobOffers = new HashSet<JobOffer>();
        }

        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<JobOffer> JobOffers { get; set; }
    }
}
