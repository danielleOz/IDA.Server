using System;
using System.Collections.Generic;

#nullable disable

namespace IDA.ServerBL.Models
{
    public partial class Service
    {
        public Service()
        {
            JobOffers = new HashSet<JobOffer>();
            WorkerServices = new HashSet<WorkerService>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<JobOffer> JobOffers { get; set; }
        public virtual ICollection<WorkerService> WorkerServices { get; set; }
    }
}
