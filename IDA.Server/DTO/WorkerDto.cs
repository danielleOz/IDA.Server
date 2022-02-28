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
        public int Id { get; set; }
        public double RadiusKm { get; set; }
        public DateTime? AvailbleUntil { get; set; }
        public bool IsAvailble { get; set; }

        public ICollection<JobOffer> WorkerJobOffers { get; set; }
        public ICollection<WorkerService> WorkerServices { get; set; }
    }
}
