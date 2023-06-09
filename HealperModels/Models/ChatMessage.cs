using System;
using System.Collections.Generic;

namespace HealperModels.Models
{
    public partial class ChatMessage
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ConsultantId { get; set; }
        public long CreateTime { get; set; }
        public string? Content { get; set; }
        public string? Sender { get; set; }
    }
}
