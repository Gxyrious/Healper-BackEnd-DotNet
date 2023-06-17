using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealperDto.OutDto
{
    public struct ClientInfo
    {
        public int? id { get; set; }
        public string? nickname { get; set; }
        public string? sex { get; set; }
        public string? userphone { get; set; }
        public int? exConsultantId { get; set; }
        public int? age { get; set; }
        public string? profile { get; set; }

        public ClientInfo(int? id, string? nickname, string? sex, string? userphone, int? exConsultantId, int? age, string? profile)
        {
            this.id = id;
            this.nickname = nickname;
            this.sex = sex;
            this.userphone = userphone;
            this.exConsultantId = exConsultantId;
            this.age = age;
            this.profile = profile;
        }
    }
}
