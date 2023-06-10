using HealperDto.OutDto;
using HealperModels.Models;

namespace HealperService
{
    public interface IUserService
    {
        IUser findUserByPhone(string phone);

        Client findClientByUserPhone(string phone);

        Consultant findConsultantByUserPhone(string phone);
    }
}