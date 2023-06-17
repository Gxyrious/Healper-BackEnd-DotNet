using HealperDto.OutDto;
using HealperModels.Models;

namespace HealperService
{
    public interface IUserService
    {
        IUser? FindUserByPhone(string phone);

        Client? FindClientByUserPhone(string phone);

        Consultant? FindConsultantByUserPhone(string phone);

        ClientInfo? FindClientInfoById(int id);

        ConsultantInfo? FindConsultantInfoById(int id);

        Client AddClientInfo(string nickname, string password, string userphone, string sex, int age);

        void UpdateClientInfo(ClientInfo client);

        void UpdateConsultantInfo(ConsultantInfo consultant);

        bool CheckPasswdWithId(int id, string userType, string password);

        void UpdateUserPasswd(int id, string userType, string password);

        List<ConsultantInfo> FindConsultantsByLabel(string label, int page, int size);

        List<ConsultantStatus> FindConsultantsWithClient(int clientId, string label, int page, int size);

    }
}