using System;
using System.Collections.Generic;

namespace HealperModels.Models
{
    public partial class ConsultHistory
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ConsultantId { get; set; }
        public long? EndTime { get; set; }
        public double Expense { get; set; }
        public long? StartTime { get; set; }
        public string Status { get; set; } = null!;
        public string? Advice { get; set; }
        public string? Summary { get; set; }
    }
}
