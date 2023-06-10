using HealperModels.Models;
using HealperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealperService.Impl
{  
    public class UserServiceImpl : IUserService
    {
        private readonly ModelContext myContext;

        public UserServiceImpl(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        public IUser findUserByPhone(string phone)
        {
            Client client = findClientByUserPhone(phone);
            if (client == null)
            {
                return findConsultantByUserPhone(phone);
            }
            else
            {
                return client;
            }
        }

        public Client findClientByUserPhone(string phone)
        {
            return myContext.Clients.FirstOrDefault(x => x.Userphone == phone);
        }

        public Consultant findConsultantByUserPhone(string phone)
        {
            return myContext.Consultants.FirstOrDefault(x => x.Userphone == phone);
        }
    }
}
