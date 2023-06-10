using System;
using System.Collections.Generic;

namespace HealperModels.Models
{
    public interface IUser
    {
        int GetId();

        string GetUserphone();

        string GetPassword();
    }
}
