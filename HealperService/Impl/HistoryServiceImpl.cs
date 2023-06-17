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
    public class HistoryServiceImpl : IHistoryService
    {
        private readonly ModelContext myContext;

        private readonly IConsultService myConsultService;

        public HistoryServiceImpl(ModelContext modelContext, IConsultService consultService)
        {
            myContext = modelContext;
            myConsultService = consultService;
        }

        public int AddConsultHistory(int clientId, int consultantId, int expense, string status)
        {
            ConsultHistory history = new();
            history.Id = myContext.ConsultHistories.Max(x => x.Id) + 1;
            history.ClientId = clientId;
            history.ConsultantId = consultantId;
            history.Expense = expense;
            if ("p" != status)
            {
                throw new Exception("Status Must Be 'p'");
            } else
            {
                history.Status = status;
            }
            myContext.ConsultHistories.Add(history);
            myContext.SaveChanges();
            return history.Id;
        }

        public List<Archive> FindArchiveByClientId(int clientId, int page, int size)
        {
            return myContext.ConsultHistories
                .Where(w => w.ClientId == clientId && w.Status == "f")
                .OrderByDescending(o => o.EndTime)
                .Skip(size * (page - 1)).Take(size)
                .Join(myContext.Consultants, ch => ch.ConsultantId, c => c.Id, (ch, c) => new Archive
                {
                    id = ch.Id,
                    consultantId = ch.ConsultantId,
                    endTime = ch.EndTime,
                    expense = ch.Expense,
                    startTime = ch.StartTime,
                    advice = ch.Advice,
                    summary = ch.Summary,
                    consultantRealName = c.Realname
                }).ToList();
        }

        Dictionary<string, int> statusPriotiry = new Dictionary<string, int>
        {
            {"p", 1 }, {"w", 2 }, {"s", 3 }, {"f", 4 }, {"c", 5 },
        };

        public List<ConsultHistory> FindRecordsByClientId(int clientId, int page, int size)
        {
            List<ConsultHistory> histories = myContext.ConsultHistories
                .Where(w => w.ClientId == clientId).ToList();
            histories.Sort((o1, o2) =>
            {
                if (statusPriotiry.ContainsKey(o1.Status) && statusPriotiry.ContainsKey(o2.Status))
                {
                    int re = statusPriotiry[o1.Status].CompareTo(statusPriotiry[o2.Status]);
                    if (re == 0)
                    {
                        if (o1.StartTime != null)
                        {
                            if (o2.StartTime == null)
                            {
                                return 1;
                            }
                            else
                            {
                                return o1.StartTime.Value.CompareTo(o2.StartTime.Value);
                            }
                        }
                    }
                    return re;
                }
                else
                {
                    throw new Exception();
                }
            });
            int endIndex = page * size;
            if (endIndex > histories.Count)
            {
                endIndex = histories.Count;
            }
            return histories.GetRange(size * (page - 1), size);
        }

        public List<ConsultOrder> FindWaitingOrdersByClientId(int clientId)
        {
            return myContext.ConsultHistories
                .Where(w => w.ClientId == clientId && (w.Status == "w" || w.Status == "p" || w.Status == "s"))
                .Join(myContext.Consultants, ch => ch.ConsultantId, c => c.Id, (ch, c) => new ConsultOrder
                {
                    id = ch.Id,
                    startTime = ch.StartTime,
                    endTime = ch.EndTime,
                    consultantId = ch.ConsultantId,
                    clientId = ch.ClientId,
                    realname = c.Realname,
                    expense = ch.Expense,
                    status = ch.Status,
                    clientSex = c.Sex,
                    clientAge = c.Age
                })
                .OrderByDescending(o => o.startTime).ToList();
        }

        public List<ConsultOrder> FindConsultOrdersByClientId(int clientId, int page, int size)
        {
            List<ConsultOrder> orders = myContext.ConsultHistories
                .Where(w => w.ClientId == clientId)
                .Join(myContext.Consultants, ch => ch.ConsultantId, c => c.Id, (ch, c) => new ConsultOrder
                {
                    id = ch.Id,
                    startTime = ch.StartTime,
                    endTime = ch.EndTime,
                    consultantId = ch.ConsultantId,
                    clientId = ch.ClientId,
                    realname = c.Realname,
                    expense = ch.Expense,
                    status = ch.Status,
                    clientSex = c.Sex,
                    clientAge = c.Age
                }).ToList();
            orders.Sort((o1, o2) =>
            {
                if (statusPriotiry.ContainsKey(o1.status!) && statusPriotiry.ContainsKey(o2.status!))
                {
                    int re = statusPriotiry[o1.status!].CompareTo(statusPriotiry[o2.status!]);
                    if (re == 0)
                    {
                        if (o1.startTime != null)
                        {
                            if (o2.startTime == null)
                            {
                                return 1;
                            }
                            else
                            {
                                return o1.startTime.Value.CompareTo(o2.startTime.Value);
                            }
                        }
                    }
                    return re;
                }
                else
                {
                    throw new Exception();
                }
            });
            int endIndex = page * size;
            if (endIndex > orders.Count)
            {
                endIndex = orders.Count;
            }
             return orders.GetRange(size * (page - 1), size);
        }

        public string? FindQrCodeByHistoryId(int historyId)
        {
            return myContext.ConsultHistories
                .Where(w => w.Id == historyId)
                .Join(myContext.Consultants, ch => ch.ConsultantId, c => c.Id, (ch, c) => c.QrCodeLink)
                .FirstOrDefault();
        }

        public bool UpdateHistoryStatusById(int historyId, string status)
        {
            try
            {
                var WoCaoNiMa = myContext.ConsultHistories.Single(s => s.Id == historyId);
                WoCaoNiMa.Status = status;
                myContext.SaveChanges();
                if ("s" == status)
                {
                    myConsultService.StartConsultation(historyId, DateTimeOffset.Now.ToUnixTimeSeconds());
                }
                else if ("f" == status)
                {
                    myConsultService.EndConsultation(historyId, DateTimeOffset.Now.ToUnixTimeSeconds());
                }
                return true;
            } catch (Exception)
            {
                return false;
            }
        }
    }
}
