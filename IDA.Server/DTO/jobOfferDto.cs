using IDA.ServerBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDA.Server.DTO
{
    public class JobOfferDto
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int? ChosenWorkerId { get; set; }
        public int UserId { get; set; }
        public DateTime PublishDate { get; set; }
        public int StatusId { get; set; }
        public string Description { get; set; }
        public string WorkerReviewDescriptipon { get; set; }
        public int? WorkerReviewRate { get; set; }
        public DateTime? WorkerReviewDate { get; set; }

        public virtual User ChosenWorker { get; set; }
        public virtual Service Service { get; set; }
        public virtual JobOfferStatus Status { get; set; }
        public virtual User User { get; set; }

        public JobOfferDto()
        {
           
            }
        public JobOfferDto(JobOffer job)
        {
            Id = job.Id;
            ServiceId = job.ServiceId;
            ChosenWorkerId = job.ChosenWorkerId;
            UserId = job.UserId;
            PublishDate = job.PublishDate;
            StatusId = job.StatusId;
            Description = job.Description;
            WorkerReviewDescriptipon = job.WorkerReviewDescriptipon;
            WorkerReviewRate = job.WorkerReviewRate;
            WorkerReviewDate = job.WorkerReviewDate;
            ChosenWorker = job.ChosenWorker.IdNavigation;
            Service = job.Service;
            Status = job.Status;
            User = job.User;
        }

        
    }
}
