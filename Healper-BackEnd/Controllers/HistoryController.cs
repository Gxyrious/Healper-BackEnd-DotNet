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
    }
}
