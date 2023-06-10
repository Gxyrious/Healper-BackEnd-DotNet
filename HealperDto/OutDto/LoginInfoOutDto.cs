using HealperModels.Models;

namespace HealperDto.OutDto
{
    public struct LoginClientInfoOutDto
    {
        public Client user { get; set; }

        public UserType userType { get; set; }

        //public String tokenName { get; set; }

        //public String tokenValue { get; set; }
    }

    public struct LoginConsultantInfoOutDto
    {
        public Consultant user { get; set; }

        public UserType userType { get; set; }

        //public String tokenName { get; set; }

        //public String tokenValue { get; set; }
    }
}
