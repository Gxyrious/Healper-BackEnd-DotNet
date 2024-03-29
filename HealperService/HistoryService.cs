﻿using HealperDto.OutDto;
using HealperModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealperService
{
    public interface IHistoryService
    {
        int AddConsultHistory(int clientId, int consultantId, int expense, string status);

        List<ConsultOrder> FindWaitingOrdersByClientId(int clientId);

        List<Archive> FindArchiveByClientId(int clientId, int page, int size);

        List<ConsultHistory> FindRecordsByClientId(int clientId, int page, int size);

        List<ConsultOrder> FindConsultOrdersByClientId(int clientId, int page, int size);

        string? FindQrCodeByHistoryId(int historyId);

        bool UpdateHistoryStatusById(int historyId, string status);

        int FindArchiveNumByClientId(int clientId);

        int GetOrderNumByClientId(int clientId);

        void DeleteOldWaitingOrdersByIds(List<int> ids);

        List<ConsultOrder> FindConsultOrdersByConsultantId(int consultantId, int page, int size);

        int GetOrderNumByConsultantId(int consultantId);

        void WriteClientArchive(int historyId, string adviceURL, string summaryURL);
    }
}
