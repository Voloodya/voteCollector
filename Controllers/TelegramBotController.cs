using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TLmessanger.Models;
using voteCollector.Data;
using voteCollector.DTO;
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
                responseMessage = new ResponseMessageData { status = "Invalid phone number" };
                return Ok(responseMessage);
            }

            string textQrCode;
            Byte[] byteCode = null;

            if (friend != null) {
                textQrCode = friend.TextQRcode;
            }
            else
            {
                responseMessage = new ResponseMessageData { status = "Not found" };
                return Ok(responseMessage);
            }

            if(friend.ByteQrcode != null)
            {
                byteCode = friend.ByteQrcode;
            }

            if(textQrCode != null && byteCode != null)
            {
                responseMessage = new ResponseMessageData { phoneNumber = friend.Telephone, textMessage = textQrCode, byteQrcode = byteCode, status = "Ok" };
            }
            else if (textQrCode != null)
            {
                responseMessage = new ResponseMessageData { phoneNumber = friend.Telephone, textMessage = textQrCode};
            }
            else
            {
                responseMessage = new ResponseMessageData { textMessage = "Участнику с данным номером телефона не присвоен QR-код!", status = "Ok" };
            }
            //jsonResponse = JsonSerializer.Serialize(responseMessage);

            return Ok(responseMessage);
        }

        [HttpPost("RegistrationFromTelegram")]
        public async Task<IActionResult> RegistrationFromTelegram([FromBody] MessageRegisterUser registerUser)
        {
            Friend friend;
            ResponseMessageData responseMessage;
            string numberPhone = registerUser.phoneNumber != null ? ServicePhoneNumber.LeaveOnlyNumbers(registerUser.phoneNumber) : null;

            if (numberPhone != null)
            {
                friend = _serviceFriends.FindUserByPhoneNumber(numberPhone);
            }
            else
            {
                responseMessage = new ResponseMessageData { status = " Некорректный номер телефона" };
                return Ok(responseMessage);
            }
            if(friend != null)
            {
                responseMessage = new ResponseMessageData { status = " Участник с данным номером телефона уже зарегистрован!" };
                return Ok(responseMessage);
            }
            else if (_serviceFriends.FindFriendByUserNameMessanger(registerUser.userNameMessanger) != null)
            {
                responseMessage = new ResponseMessageData { status = " С данного аккаунта уже была произведена регистрация ранее!"};
                return Ok(responseMessage);
            }
            else
            {                
                DateTime dateRegistration = DateTime.Today;

                FriendDTO friendDTO = new FriendDTO { FamilyName = registerUser.famileName, Name = registerUser.name, PatronymicName = registerUser.patronimicName,
                                                      Telephone = registerUser.phoneNumber, userNameMessanger = registerUser.userNameMessanger,
                                                      FieldActivityName = registerUser.fieldActivityName, Organization = registerUser.organization,
                                                      Group = registerUser.group, LoginUsers = registerUser.user, TextQRcode = registerUser.phoneNumber};

                Friend newFriend = null;
                try
                {
                    newFriend = _serviceFriends.CreateFreandMessanger(friendDTO, dateRegistration);
                }
                catch (Exception ex)
                {
                    responseMessage = new ResponseMessageData { status = " Ошибка создания участника." };
                    return Ok(responseMessage);
                }
                bool save = false;

                if (newFriend != null)
                {
                    try
                    {
                        save = await _serviceFriends.SaveNewFriends(newFriend);
                    }
                    catch (Exception ex)
                    {
                        responseMessage = new ResponseMessageData { status = " Ошибка сохранения данных об участнике." };
                        return Ok(responseMessage);
                    }
                }

                if (save)
                {
                    responseMessage = new ResponseMessageData { status = " Регистрация прошла успешно!" };

                    if (newFriend.TextQRcode != null && newFriend.ByteQrcode != null)
                    {
                        responseMessage.byteQrcode = newFriend.ByteQrcode;
                        responseMessage.textMessage = newFriend.TextQRcode;
                    }
                    else if (newFriend.TextQRcode != null)
                    {
                        responseMessage.textMessage = newFriend.TextQRcode;
                    }
                    else
                    {
                        responseMessage.textMessage = " Участник зарегистрирован, но QR код сгенерировать не получилось. Обратитесь пожалуйста к администратору сервиса!";
                    }
                    return Ok(responseMessage);
                }
                else
                {
                    responseMessage = new ResponseMessageData { status = " Ваши данные не удалось сохранить. Регистрация не выполнена!" };
                    return Ok(responseMessage);
                }
                
            }
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
