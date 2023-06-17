using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealperDto.OutDto
{
    public struct ConsultOrder
    {
        public int? id { get; set; }
        public long? startTime { get; set; }
        public long? endTime { get; set; }
        public int? consultantId { get; set; }
        public int? clientId { get; set; }
        public string? realname { get; set; }
        public int? expense { get; set; }
        public string? status { get; set; }
        public string? clientSex { get; set; }
        public int? clientAge { get; set; }
        public string? consultantLabel { get; set; }
        public int? consultantAge { get; set; }
        public string? consultantSex { get; set; }
        public string? consultantProfile { get; set; }
    }
}
