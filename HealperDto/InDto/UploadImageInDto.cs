using HealperDto.OutDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealperDto.InDto
{
    public struct UploadImageInDto
    {
        public int id { set; get; }

        public string base64 { set; get; }

        public string userType { set; get; }
    }
}
