using System;
using System.Collections.Generic;

namespace HealperModels.Models
{
    public partial class PsychologyScale
    {
        public PsychologyScale()
        {
            ScaleRecords = new HashSet<ScaleRecord>();
        }

        public int Id { get; set; }
        public int QuesNum { get; set; }
        public string Content { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public string? Summary { get; set; }
        public string? Subjective { get; set; }

        public virtual ICollection<ScaleRecord> ScaleRecords { get; set; }
    }
}
