using HealperDto.OutDto;
using HealperModels;
using HealperModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealperService.Impl
{
    public class ConsultServiceImpl : IConsultService
    {
        private readonly ModelContext myContext;

        public ConsultServiceImpl(ModelContext modelContext)
        {
            myContext = modelContext;
        }

        public bool EndConsultation(int orderId, long endTime)
        {
            try
            {
                var order = myContext.ConsultHistories.Single(s => s.Id == orderId);
                order.EndTime = endTime;
                myContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool StartConsultation(int orderId, long startTime)
        {
            try
            {
                var order = myContext.ConsultHistories.Single(s => s.Id == orderId);
                order.StartTime = startTime;
                myContext.SaveChanges();
                return true;
            } catch (Exception)
            {
                return false;
            }
        }
        public List<ChatMessage> FindChatMessagesByClientIdAndConsultantId(int clientId, int consultantId, int page, int size)
        {
            return myContext.ChatMessages.Where(w => w.ClientId == clientId && w.ConsultantId == consultantId).ToList();
        }

        public ChatMessage FindMessageById(int messageId)
        {
            return myContext.ChatMessages.Single(s => s.Id == messageId);
        }
    }
}
