using System;
using System.Collections.Generic;

#nullable disable

namespace IDA.ServerBL.Models
{
    public partial class Customer
    {
        public Customer()
        {
            JobOffers = new HashSet<JobOffer>();
            Reviews = new HashSet<Review>();
        }

        public int Cid { get; set; }
        public string UserName { get; set; }
        public int Lid { get; set; }

        public virtual Location LidNavigation { get; set; }
        public virtual User UserNameNavigation { get; set; }
        public virtual ICollection<JobOffer> JobOffers { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
