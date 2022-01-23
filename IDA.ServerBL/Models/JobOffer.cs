using System;
using System.Collections.Generic;

#nullable disable

namespace IDA.ServerBL.Models
{
    public partial class JobOffer
    {
        public JobOffer()
        {
            Reviews = new HashSet<Review>();//tesr
        }

        public int Jid { get; set; }
        public int Wid { get; set; }
        public int Cid { get; set; }
        public int Swid { get; set; }
        public int Time { get; set; }
        public int Status { get; set; }
        public int Rid { get; set; }

        public virtual Customer CidNavigation { get; set; }
        public virtual Review RidNavigation { get; set; }
        public virtual Status StatusNavigation { get; set; }
        public virtual WorkerService Sw { get; set; }
        public virtual Worker WidNavigation { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
