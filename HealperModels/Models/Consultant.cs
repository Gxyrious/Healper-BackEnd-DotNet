using System;
using System.Collections.Generic;

namespace HealperModels.Models
{
    public partial class Consultant
    {
        public int Id { get; set; }
        public string Password { get; set; } = null!;
        public string? QrCodeLink { get; set; }
        public string Realname { get; set; } = null!;
        public string? Sex { get; set; }
        public string Userphone { get; set; } = null!;
        public int? Age { get; set; }
        public short? Expense { get; set; }
        public string? Label { get; set; }
        public string? Profile { get; set; }
    }
}
