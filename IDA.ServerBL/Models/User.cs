using System;
using System.Collections.Generic;

#nullable disable

namespace IDA.ServerBL.Models
{
    public partial class User
    {
        public User()
        {
            ChatMessageRecievers = new HashSet<ChatMessage>();
            ChatMessageSenders = new HashSet<ChatMessage>();
            JobOffers = new HashSet<JobOffer>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserPswd { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Apartment { get; set; }
        public string HouseNumber { get; set; }
        public DateTime Birthday { get; set; }
        public bool IsWorker { get; set; }

        public virtual Worker Worker { get; set; }
        public virtual ICollection<ChatMessage> ChatMessageRecievers { get; set; }
        public virtual ICollection<ChatMessage> ChatMessageSenders { get; set; }
        public virtual ICollection<JobOffer> JobOffers { get; set; }
    }
}
