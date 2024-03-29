﻿using System;
using System.Collections.Generic;

namespace HealperModels.Models
{
    public partial class Client : IUser
    {
        public int Id { get; set; }
        public string Nickname { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Sex { get; set; }
        public string Userphone { get; set; } = null!;
        public int? ExConsultantId { get; set; }
        public int? Age { get; set; }
        public string Profile { get; set; } = null!;

        public int GetId()
        {
            return Id;
        }

        public string GetPassword()
        {
            return Password;
        }

        public string GetUserphone()
        {
            return Userphone;
        }
    }
}
