using System;
using System.Collections.Generic;

#nullable disable

namespace IDA.ServerBL.Models
{
    public partial class JobOffer
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

        public virtual Worker ChosenWorker { get; set; }
        public virtual Service Service { get; set; }
        public virtual JobOfferStatus Status { get; set; }
        public virtual User User { get; set; }
    }
}
