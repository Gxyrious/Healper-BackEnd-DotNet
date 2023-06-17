using ExternalInterfaces;
using HealperDto.InDto;
using HealperDto.OutDto;
using HealperModels.Models;
using HealperResponse;
using HealperService;
using Microsoft.AspNetCore.Mvc;

namespace Healper_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService myHistoryService;

        private readonly IUserService myUserService;

        public HistoryController(IHistoryService historyService, IUserService userService)
        {
            myHistoryService = historyService;
            myUserService = userService;
        }

        [HttpPost("add")]
        public ResponseEntity AddHistory(ConsultRecordInDto inDto)
        {
            try
            {
                List<ConsultOrder> waitingOrders = myHistoryService.FindWaitingOrdersByClientId(inDto.clientId);
                if (waitingOrders.Count == 0)
                {
                    int historyId = myHistoryService.AddConsultHistory(inDto.clientId, inDto.consultantId, inDto.expense, inDto.status);
                    return ResponseEntity.OK().Body(historyId);
                } else
                {
                    return ResponseEntity.ERR().Body(waitingOrders);
                }
            } catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpGet("archives")]
        public ResponseEntity GetArchivesByClientId(int clientId, int page, int size)
        {
            try
            {
                List<Archive> archives = myHistoryService.FindArchiveByClientId(clientId, page, size);
                return ResponseEntity.OK().Body(archives);
            } catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpGet("records")]
        public ResponseEntity GetConsultRecordsByClientId(int clientId, int page, int size)
        {
            try
            {
                List<ConsultHistory> records = myHistoryService.FindRecordsByClientId(clientId, page, size);
                return ResponseEntity.OK().Body(records);
            } catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpGet("orders")]
        public ResponseEntity GetConsultOrdersByClientId(int clientId, int page, int size)
        {
            try
            {
                List<ConsultOrder> orders = myHistoryService.FindConsultOrdersByClientId(clientId, page, size);
                return ResponseEntity.OK().Body(orders);
            }
            catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpGet("pay")]
        public ResponseEntity GetQrCodeByHistoryId(int historyId)
        {
            string? qrCode = myHistoryService.FindQrCodeByHistoryId(historyId);
            if (qrCode != null)
            {
                return ResponseEntity.OK().Body(qrCode);
            } else
            {
                return ResponseEntity.ERR("History Not Found");
            }
        }

        [HttpPut("status")]
        public ResponseEntity UpdateHistoryStatusById(HistoryStatusInDto inDto)
        {
            string status = inDto.status;
            HashSet<char> statusSet = new HashSet<char>();
            statusSet.Add('w');
            statusSet.Add('p');
            statusSet.Add('f');
            statusSet.Add('s');
            statusSet.Add('c');
            if (status.Length == 1 && statusSet.Contains(status[0]))
            {
                // 数据无误
                bool result = myHistoryService.UpdateHistoryStatusById(inDto.historyId, status);
                if (result)
                {
                    return ResponseEntity.OK().Body(status);
                } else
                {
                    return ResponseEntity.ERR("HistoryId Not Found");
                }
            } else
            {
                return ResponseEntity.ERR("Data 'status' isn't one of {'w', 'p', 'f', 's', 'c'}");
            }
        }

        [HttpGet("archive/sum")]
        public ResponseEntity GetArchiveNumByClientId(int clientId)
        {
            try
            {
                int num = myHistoryService.FindArchiveNumByClientId(clientId);
                return ResponseEntity.OK().Body(num);
            } catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpGet("order/sum")]
        public ResponseEntity GetOrderNumByClientId(int clientId)
        {
            try
            {
                int num = myHistoryService.GetOrderNumByClientId(clientId);
                return ResponseEntity.OK().Body(num);
            } catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpGet("order/waiting")]
        public ResponseEntity GetWaitingConsultOrder(int clientId)
        {
            try
            {
                List<ConsultOrder> waitingOrders = myHistoryService.FindWaitingOrdersByClientId(clientId);
                if (waitingOrders.Count == 0)
                {
                    return ResponseEntity.OK("No Waiting");
                } else
                {
                    if (waitingOrders.Count > 1)
                    {
                        List<int> ids = new();
                        for (int i = 1; i < waitingOrders.Count; i++)
                        {
                            ids.Add(waitingOrders[i].id!.Value);
                        }
                        myHistoryService.DeleteOldWaitingOrdersByIds(ids);
                    }
                    ConsultOrder order = waitingOrders.First();
                    ConsultantInfo consultantInfo = myUserService.FindConsultantInfoById(order.consultantId!.Value)!.Value;
                    order.consultantLabel = consultantInfo.label;
                    order.consultantAge = consultantInfo.age;
                    order.consultantProfile = consultantInfo.profile;
                    order.consultantSex = consultantInfo.sex;
                    return ResponseEntity.OK().Body(order);
                }
            } catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpGet("consultant")]
        public ResponseEntity GetConsultantHistory(int consultantId, int page, int size)
        {
            try
            {
                List<ConsultOrder> orders = myHistoryService.FindConsultOrdersByConsultantId(consultantId, page, size);
                return ResponseEntity.OK().Body(orders);
            } catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpGet("consultant/sum")]
        public ResponseEntity GetConsultantHistoryNum(int consultantId)
        {
            try
            {
                int num = myHistoryService.GetOrderNumByConsultantId(consultantId);
                return ResponseEntity.OK().Body(num);
            } catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpPost("archive")]
        public ResponseEntity WriteClientArchive(ArchiveInDto inDto)
        {
            try
            {
                int histroyId = inDto.id;
                string aBase64 = inDto.adviceBase64;
                string sBase64 = inDto.summaryBase64;

                MemoryStream aStream = new(Convert.FromBase64String(aBase64));
                MemoryStream sStream = new(Convert.FromBase64String(sBase64));

                string advicePath = "advice-" + histroyId + ".html";
                string summaryPath = "summary-" + histroyId + ".html";

                string adviceURL = OssHelp.UploadStream(aStream, advicePath);
                string summaryURL = OssHelp.UploadStream(sStream, summaryPath);

                myHistoryService.WriteClientArchive(histroyId, advicePath, summaryPath);
                Dictionary<string, string> urls = new Dictionary<string, string>();
                urls.Add("adviceURL", adviceURL);
                urls.Add("summaryURL", summaryURL);
                return ResponseEntity.OK().Body(urls);
            } catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }
    }
}
