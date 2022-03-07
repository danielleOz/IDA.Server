using System;
using System.Collections.Generic;

#nullable disable

namespace IDA.ServerBL.Models
{
    public partial class Review
    {
        public Review()
        {
            JobOffers = new HashSet<JobOffer>();
        }

        public int Rid { get; set; }
        public int Sid { get; set; }
        public string Sname { get; set; }
        public DateTime Date { get; set; }
        public int Rate { get; set; }
        public int Cid { get; set; }
        public int Wid { get; set; }
        public int Jid { get; set; }

        public virtual Customer CidNavigation { get; set; }
        public virtual JobOffer JidNavigation { get; set; }
        public virtual Worker WidNavigation { get; set; }
        public virtual ICollection<JobOffer> JobOffers { get; set; }
    }
}
