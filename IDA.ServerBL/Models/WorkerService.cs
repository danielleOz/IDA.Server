using System;
using System.Collections.Generic;

#nullable disable

namespace IDA.ServerBL.Models
{
    public partial class WorkerService
    {
        public WorkerService()
        {
            JobOffers = new HashSet<JobOffer>();
        }

        public int Swid { get; set; }
        public int Sid { get; set; }
        public int Wid { get; set; }
        public double Price { get; set; }

        public virtual Service SidNavigation { get; set; }
        public virtual Worker WidNavigation { get; set; }
        public virtual ICollection<JobOffer> JobOffers { get; set; }
    }
}
