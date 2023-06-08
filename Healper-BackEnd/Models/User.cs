using System;
using System.Collections.Generic;

namespace Healper_BackEnd.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public DateTime? Time { get; set; }
    }
}
