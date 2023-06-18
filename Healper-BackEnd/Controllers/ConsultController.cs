using HealperDto.InDto;
using HealperModels.Models;
using HealperResponse;
using HealperService;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;

namespace Healper_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultController : ControllerBase
    {
        private readonly IConsultService myConsultService;

        public ConsultController(IConsultService consultService)
        {
            myConsultService = consultService;
        }

        [HttpPut("start")]
        public ResponseEntity StartConsultation(ConsultTimeInDto inDto)
        {
            long startTime = inDto.time;
            if (myConsultService.StartConsultation(inDto.orderId, startTime))
            {
                return ResponseEntity.OK("Consultation Start");
            } else
            {
                return ResponseEntity.ERR("Failed To Start");
            }
        }

        [HttpPut("end")]
        public ResponseEntity EndConsultation(ConsultTimeInDto inDto)
        {
            long endTime = inDto.time;
            if (myConsultService.StartConsultation(inDto.orderId, endTime))
            {
                return ResponseEntity.OK("Consultation End");
            }
            else
            {
                return ResponseEntity.ERR("Failed To End");
            }
        }

        [HttpGet("records")]
        public ResponseEntity GetChatRecords(int clientId, int consultantId, int page, int size)
        {
            try
            {
                List<ChatMessage> messages = myConsultService.FindChatMessagesByClientIdAndConsultantId(clientId, consultantId, page, size);
                return ResponseEntity.OK().Body(messages);
            } catch (Exception err)
            {
                return ResponseEntity.ERR().Body(err);
            }
        }

        [HttpGet("record")]
        public ResponseEntity GetChatRecord(int msgId)
        {
            try
            {
                ChatMessage message = myConsultService.FindMessageById(msgId);
                return ResponseEntity.OK().Body(message);
            } catch (Exception)
            {
                return ResponseEntity.ERR();
            }
        }

        [HttpGet("ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await HandleWebSocketConnection(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }

        private async Task HandleWebSocketConnection(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                await ProcessWebSocketMessage(message, webSocket);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        private async Task ProcessWebSocketMessage(string message, WebSocket webSocket)
        {
            var serverResponse = $"Server: Received message '{message}'.";
            var serverResponseBytes = Encoding.UTF8.GetBytes(serverResponse);
            await webSocket.SendAsync(new ArraySegment<byte>(serverResponseBytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}