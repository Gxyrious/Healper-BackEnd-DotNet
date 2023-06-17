using HealperDto.OutDto;
using HealperModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealperService
{
    public interface IConsultService
    {
        bool StartConsultation(int orderId, long startTime);

        bool EndConsultation(int orderId, long endTime);

        List<ChatMessage> FindChatMessagesByClientIdAndConsultantId(int clientId, int consultantId, int page, int size);

        ChatMessage FindMessageById(int messageId);
    }
}
