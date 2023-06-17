using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealperDto.OutDto
{
    public struct Archive
    {
        public int? id { get; set; }
        public int? consultantId { get; set; }
        public long? endTime { get; set; }
        public int? expense { get; set; }
        public long? startTime { get; set; }
        public string? advice { get; set; }
        public string? summary { get; set; }
        public string? consultantRealName { get; set; }
    }
}
