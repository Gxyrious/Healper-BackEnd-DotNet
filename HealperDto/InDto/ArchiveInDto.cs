using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealperDto.InDto
{
    public struct ArchiveInDto
    {
        public int id { get; set; }
        public string adviceBase64 { get; set; }
        public string summaryBase64 { get; set; }
    }
}
