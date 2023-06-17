using HealperModels.Models;
using HealperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealperDto.OutDto;
using Microsoft.EntityFrameworkCore;

namespace HealperService.Impl
{  
    public class UserServiceImpl : IUserService
    {
        private readonly ModelContext myContext;

        public UserServiceImpl(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        public IUser? FindUserByPhone(string phone)
        {
            Client? client = FindClientByUserPhone(phone);
            if (client == null)
            {
                return FindConsultantByUserPhone(phone);
            }
            else
            {
                return client;
            }
        }

        public Client? FindClientByUserPhone(string phone)
        {
            return myContext.Clients.FirstOrDefault(x => x.Userphone == phone);
        }

        public Consultant? FindConsultantByUserPhone(string phone)
        {
            return myContext.Consultants.FirstOrDefault(x => x.Userphone == phone);
        }

        public ClientInfo? FindClientInfoById(int id)
        {
            try
            {
                return myContext.Clients
                               .Select(s => new ClientInfo(s.Id, s.Nickname, s.Sex, s.Userphone, s.ExConsultantId, s.Age, s.Profile))
                               .AsEnumerable()
                               .Where(w => w.id == id)
                               .Single();
            } catch (Exception)
            {
                return null;
            }
        }

        public ConsultantInfo? FindConsultantInfoById(int id)
        {
            try
            {
                return myContext.Consultants
                    .Select(s => new ConsultantInfo(s.Id, s.QrCodeLink, s.Realname, s.Sex, s.Userphone, s.Age, s.Expense, s.Label, s.Profile))
                    .AsEnumerable()
                    .Where(w => w.id == id)
                    .Single();

            } catch (Exception)
            {
                return null;
            }
        }

        public Client AddClientInfo(string nickname, string password, string userphone, string sex, int age)
        {
            Client newClient = new Client();
            newClient.Nickname = nickname;
            newClient.Password = password;
            newClient.Sex = sex;
            newClient.Userphone = userphone;
            newClient.Age = age;
            newClient.Id = myContext.Clients.Max(s => s.Id) + 1;
            myContext.Clients.Add(newClient);
            myContext.SaveChanges();
            return newClient;
        }

        public void UpdateClientInfo(ClientInfo client)
        {
            Client updatedClient = myContext.Clients.SingleOrDefault(s => s.Id == client.id)!;
            if (client.nickname != null)
            {
                updatedClient.Nickname = client.nickname;
            }
            if (client.sex != null)
            {
                updatedClient.Sex = client.sex;
            }
            if (client.age != null)
            {
                updatedClient.Age = client.age;
            }
            if (client.profile != null)
            {
                updatedClient.Profile = client.profile;
            }
            myContext.SaveChanges();
        }

        public void UpdateConsultantInfo(ConsultantInfo consultant)
        {
            Consultant updatedConsultant = myContext.Consultants.SingleOrDefault(s => s.Id == consultant.id)!;
            if (consultant.realname != null)
            {
                updatedConsultant.Realname = consultant.realname;
            }
            if (consultant.sex != null)
            {
                updatedConsultant.Sex = consultant.sex;
            }
            if (consultant.age != null)
            {
                updatedConsultant.Age = consultant.age;
            }
            if (consultant.profile != null)
            {
                updatedConsultant.Profile = consultant.profile;
            }
            if (consultant.expense != null)
            {
                updatedConsultant.Expense = consultant.expense;
            }
            if (consultant.label != null)
            {
                updatedConsultant.Label = consultant.label;
            }
            myContext.SaveChanges();
        }

        public bool CheckPasswdWithId(int id, string userType, string password)
        {
            string? realPassword = null;
            if (userType == "client")
            {
                realPassword = myContext.Clients.Single(s => s.Id == id).Password;
            } else if (userType == "consultant")
            {
                realPassword = myContext.Consultants.Single(s => s.Id == id).Password;
            } else
            {
                throw new Exception("UserType Error");
            }
            return realPassword != null && realPassword == password;
        }

        public void UpdateUserPasswd(int id, string userType, string password)
        {
            if (userType == "client")
            {
                Client updatedClient = myContext.Clients.SingleOrDefault(s => s.Id == id)!;
                updatedClient.Password = password;
                myContext.SaveChanges();
            } else if (userType == "consultant")
            {
                Consultant updatedConsultant = myContext.Consultants.SingleOrDefault(s => s.Id == id)!;
                updatedConsultant.Password = password;
                myContext.SaveChanges();
            } else
            {
                throw new Exception("UserType Error");
            }
        }

        public List<ConsultantInfo> FindConsultantsByLabel(string label, int page, int size)
        {
            return myContext.Consultants
                .Where(w => EF.Functions.Like(w.Label ?? "", $"%{label}%"))
                .Skip(size * (page - 1)).Take(size)
                .Select(s => new ConsultantInfo
                {
                    id = s.Id,
                    qrCodeLink = s.QrCodeLink,
                    realname = s.Realname,
                    sex = s.Sex,
                    userphone = s.Userphone,
                    age = s.Age,
                    expense = s.Expense,
                    label = s.Label,
                    profile = s.Profile,
                }).ToList();
        }

        public List<ConsultantStatus> FindConsultantsWithClient(int clientId, string label, int page, int size)
        {
            List<ConsultantInfo> consultants = myContext.Consultants
                .Where(w => EF.Functions.Like(w.Label ?? "", $"%{label}%"))
                .Skip(size * (page - 1)).Take(size)
                .Select(s => new ConsultantInfo
                {
                    id = s.Id,
                    qrCodeLink = s.QrCodeLink,
                    realname = s.Realname,
                    sex = s.Sex,
                    userphone = s.Userphone,
                    age = s.Age,
                    expense = s.Expense,
                    label = s.Label,
                    profile = s.Profile,
                }).ToList();
            List<ConsultantStatus> consultantsWithClient = new List<ConsultantStatus>();
            foreach (ConsultantInfo consultant in consultants)
            {
                ConsultantStatus newConsultantInfo = new ConsultantStatus();
                newConsultantInfo.info = consultant;
                ConsultHistory? history = myContext.ConsultHistories
                    .Where(w => w.ClientId == clientId && w.ConsultantId == consultant.id && (w.Status == "w" || w.Status == "p" || w.Status == "s"))
                    .OrderByDescending(o => o.Status).FirstOrDefault();
                if (history == null)
                {
                    newConsultantInfo.status = "0";
                } else
                {
                    newConsultantInfo.historyId = history.Id;
                    if (history.Status == "p")
                    {
                        newConsultantInfo.status = "1";
                    } else
                    {
                        newConsultantInfo.status = "2";
                    }
                }
                consultantsWithClient.Add(newConsultantInfo);
            }
            return consultantsWithClient;
        }

    }
}
