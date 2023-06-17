using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealperDto.InDto
{
    public struct ScaleRecordInDto
    {
        public int clientId { get; set; }
        public long endTime { get; set; }
        public sbyte isHidden { get; set; }
        public int scaleId { get; set; }
        public string record { get; set; }
        public string? subjective { get; set; }
    }
}
