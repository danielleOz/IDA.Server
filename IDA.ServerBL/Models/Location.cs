using System;
using System.Collections.Generic;

#nullable disable

namespace IDA.ServerBL.Models
{
    public partial class Location
    {
        public Location()
        {
            Customers = new HashSet<Customer>();
            Workers = new HashSet<Worker>();
        }

        public int Lid { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Worker> Workers { get; set; }
    }
}
