using System;
using System.Collections.Generic;

namespace Healper_BackEnd.Models
{
    public partial class Client
    {
        public int Id { get; set; }
        public string Nickname { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Sex { get; set; }
        public string Userphone { get; set; } = null!;
        public int? ExConsultantId { get; set; }
        public int? Age { get; set; }
        public string Profile { get; set; } = null!;
    }
}
