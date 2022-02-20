using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDA.ServerBL.Models;

namespace IDA.Server.DTO
{
    public class WorkerDto:User
    {
        public WorkerDto()
        {
            JobOffers = new HashSet<JobOffer>();
            WorkerServices = new HashSet<WorkerService>();
        }

        public double RadiusKm { get; set; }
        public DateTime IsAvailbleUntil { get; set; }

        public ICollection<JobOffer> WorkerJobOffers { get; set; }
        public ICollection<WorkerService> WorkerServices { get; set; }
    }
}
