using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealperDto.OutDto
{
    public struct ScaleRecordInfo
    {
        public int scaleRecordId {  get; set; }
        public long endTime { get; set; }
        public sbyte isHidden { get; set; }
        public int scaleId { get; set; }
        public string record { get; set; }
        public string scaleName { get; set; }
    }
}
