using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealperDto.OutDto
{
    public struct ConsultantInfo
    {
        public int? id { get; set; }
        public string? qrCodeLink { get; set; }
        public string? realname { get; set; }
        public string? sex { get; set; }
        public  string? userphone { get; set; }
        public  int? age { get; set; }
        public short? expense { get; set; }
        public  string? label { get; set; }
        public string? profile { get; set; }

        public ConsultantInfo(int? id, string? qrCodeLink, string? realname, string? sex, string? userphone, int? age, short? expense, string? label, string? profile)
        {
            this.id = id;
            this.qrCodeLink = qrCodeLink;
            this.realname = realname;
            this.sex = sex;
            this.userphone = userphone;
            this.age = age;
            this.expense = expense;
            this.label = label;
            this.profile = profile;
        }
    }
}
