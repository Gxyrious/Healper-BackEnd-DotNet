using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealperDto.OutDto
{
    public struct ConsultantStatus
    {
        public ConsultantInfo info { get; set; }
        public string status { get; set; }
        public int historyId { get; set; }
    }
}
