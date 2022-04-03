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
            WorkerJobOffers = new HashSet<JobOffer>();
            WorkerServices = new HashSet<WorkerService>();
        }
        public int Id { get; set; }
        public double RadiusKm { get; set; }
        public DateTime? AvailbleUntil { get; set; }
        //public bool IsAvailble { get; set; }

        public ICollection<JobOffer> WorkerJobOffers { get; set; }
        public ICollection<WorkerService> WorkerServices { get; set; }

        public WorkerDto(Worker w)
        {
            Id = w.Id;
            Email = w.IdNavigation.Email;
            FirstName = w.IdNavigation.FirstName;
            LastName = w.IdNavigation.LastName;
            UserPswd = w.IdNavigation.UserPswd;
            City = w.IdNavigation.City;
            Street = w.IdNavigation.Street;
            Apartment = w.IdNavigation.Apartment;
            HouseNumber = w.IdNavigation.HouseNumber;
            Birthday = w.IdNavigation.Birthday;
            IsWorker = w.IdNavigation.IsWorker;
            //IsAvailble = w.IsAvailble;
            AvailbleUntil = w.AvailbleUntil;
            RadiusKm = w.RadiusKm;
            WorkerJobOffers = w.JobOffers;
            WorkerServices = w.WorkerServices;

            
        }
    }
}
