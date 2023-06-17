using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealperDto.InDto
{
    public struct ConsultRecordInDto
    {
        public int clientId {  get; set; }
        public int consultantId { get; set; }
        public int expense { get; set; }
        public string status { get; set; }
    }
}
