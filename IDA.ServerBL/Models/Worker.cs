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
            WorkerServices = new HashSet<WorkerService>();
        }

        public int Id { get; set; }
        public double RadiusKm { get; set; }
        public DateTime IsAvailbleUntil { get; set; }

        public virtual User IdNavigation { get; set; }
        public virtual ICollection<JobOffer> JobOffers { get; set; }
        public virtual ICollection<WorkerService> WorkerServices { get; set; }
    }
}
