using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly VoterCollectorContext _context;
        private readonly ILogger<TelegramBotController> _logger;
        private ServiceFriends _serviceFriends;

        public TelegramBotController(VoterCollectorContext context, ILogger<TelegramBotController> logger)
        {
            _context = context;
            _logger = logger;
            _serviceFriends = new ServiceFriends();
        }

        [HttpPost("AcceptReplyMessageBot")]
        public async Task<IActionResult> AcceptReplyMessageBot([FromBody] MessageData messageData)
        {
            Friend friend = _serviceFriends.FindUserByPhoneNumber(ServicePhoneNumber.LeaveOnlyNumbers(messageData.PhoneNumber));
            

            string textQrCode = null;

            if (friend != null) {
                textQrCode = friend.TextQRcode;
            }

            ResponseMessageData responseMessage;
            if(textQrCode != null)
            {
                responseMessage = new ResponseMessageData { PhoneNumber = friend.Telephone, TextMessage = textQrCode };
            }
            else
            {
                responseMessage = new ResponseMessageData { TextMessage = "Участник с данным номером телефона не найден!" };
            }
            return Ok();
        }


        }
    }
