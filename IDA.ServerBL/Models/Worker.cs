using System;
using System.Collections.Generic;

#nullable disable

namespace IDA.ServerBL.Models
{
    public partial class Worker
    {
        public Worker()
        {
            JobOffers = new HashSet<JobOffer>();
            Reviews = new HashSet<Review>();
            WorkerServices = new HashSet<WorkerService>();
        }

        public int Wid { get; set; }
        public string UserName { get; set; }
        public int Lid { get; set; }
        public string Service { get; set; }
        public double Location { get; set; }

        public virtual Location LidNavigation { get; set; }
        public virtual User UserNameNavigation { get; set; }
        public virtual ICollection<JobOffer> JobOffers { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<WorkerService> WorkerServices { get; set; }
    }
}
