using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealperDto.InDto
{
    public struct RegisterInfoInDto
    {
        public string nickname { get; set; }
        public string password { get; set; }
        public string userPhone { get; set; }
        public string sex { get; set; }
        public int age { get; set; }
        public string code { get; set; }
    }
}
