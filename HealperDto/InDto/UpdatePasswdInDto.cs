using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealperDto.InDto
{
    public struct UpdatePasswdInDto
    {
        public int id {  get; set; }
        public string oldPasswd { get; set; }
        public string newPasswd { get; set; }
        public string userType { get; set; }
    }
}
