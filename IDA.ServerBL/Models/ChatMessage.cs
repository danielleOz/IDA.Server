using System;
using System.Collections.Generic;

#nullable disable

namespace IDA.ServerBL.Models
{
    public partial class ChatMessage
    {
        public int Id { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageDate { get; set; }
        public int SenderId { get; set; }
        public int RecieverId { get; set; }

        public virtual User Reciever { get; set; }
        public virtual User Sender { get; set; }
    }
}
