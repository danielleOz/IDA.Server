using System;
using System.Collections.Generic;

#nullable disable

namespace IDA.ServerBL.Models
{
    public partial class Service
    {
        public Service()
        {
            WorkerServices = new HashSet<WorkerService>();
        }

        public int Sid { get; set; }
        public string Name { get; set; }

        public virtual ICollection<WorkerService> WorkerServices { get; set; }
    }
}
