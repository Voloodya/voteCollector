using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TLmessanger.Models;
using voteCollector.Data;
using voteCollector.Models;
using voteCollector.Services;

namespace voteCollector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TelegramBotController : ControllerBase
    {
        private readonly ILogger<TelegramBotController> _logger;
        private ServiceFriends _serviceFriends;

        public TelegramBotController(VoterCollectorContext context, ILogger<TelegramBotController> logger)
        {
            _logger = logger;
            _serviceFriends = new ServiceFriends();
        }

        [HttpPost("AcceptReplyMessageBot")]
        public async Task<IActionResult> AcceptReplyMessageBot([FromBody] MessageData messageData)
        {
            Friend friend;
            ResponseMessageData responseMessage;
            string jsonResponse;
            string numberPhone = messageData.PhoneNumber != null ? ServicePhoneNumber.LeaveOnlyNumbers(messageData.PhoneNumber) : null;
            if (numberPhone != null && numberPhone.Length > 10)
            {
                friend = _serviceFriends.FindUserByPhoneNumber(numberPhone);
            }
            else
            {
                responseMessage = new ResponseMessageData { Status = "Invalid phone number" };
                return Ok(responseMessage);
            }

            string textQrCode;
            Byte[] byteCode = null;

            if (friend != null) {
                textQrCode = friend.TextQRcode;
            }
            else
            {
                responseMessage = new ResponseMessageData { Status = "Not found" };
                return Ok(responseMessage);
            }

            if(friend.ByteQrcode != null)
            {
                byteCode = friend.ByteQrcode;
            }

            if(textQrCode != null && byteCode != null)
            {
                responseMessage = new ResponseMessageData { PhoneNumber = friend.Telephone, TextMessage = textQrCode, ByteQrcode = byteCode };
            }
            else if (textQrCode != null)
            {
                responseMessage = new ResponseMessageData { PhoneNumber = friend.Telephone, TextMessage = textQrCode};
            }
            else
            {
                responseMessage = new ResponseMessageData { TextMessage = "Участнику с данным номером телефона не присвоен QR-код!" };
            }
            //jsonResponse = JsonSerializer.Serialize(responseMessage);

            return Ok(responseMessage);
        }

        // Функция отправки запроса на API др. сервиса 
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public async Task<string> PostRequestHttpAsync(string url, string json)
        //{
        //    using HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
        //    HttpClient httpClient = _clientFactory.CreateClient();
        //    httpClient.Timeout = TimeSpan.FromSeconds(120);
        //    using HttpResponseMessage response = await httpClient.PostAsync(url, content).ConfigureAwait(false);

        //    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        //}

    }
}
