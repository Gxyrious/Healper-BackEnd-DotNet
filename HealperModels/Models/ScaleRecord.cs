using System;
using System.Collections.Generic;

namespace HealperModels.Models
{
    public partial class ScaleRecord
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public long EndTime { get; set; }
        public sbyte IsHidden { get; set; }
        public int ScaleId { get; set; }
        public string Record { get; set; } = null!;
        public string? Subjective { get; set; }

        public virtual PsychologyScale Scale { get; set; } = null!;
    }
}
