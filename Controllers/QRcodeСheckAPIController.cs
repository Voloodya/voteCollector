using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using voteCollector.Models;
using voteCollector.Services;

namespace voteCollector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin, user")]
    public class QRcodeСheckAPIController : ControllerBase
    {
        // "/api/QRcodeСheckAPI/checkqrcode?qrText="
        [HttpGet]
        [Route("checkqrcode")]
        public async Task<IActionResult> CheckQRcode(String qrText)
        {
            if (qrText != null && qrText != "")
            {
                ServiceFriends serviceFriends = new ServiceFriends();

                Friend friendUpdate = null;
                try
                {
                    friendUpdate = serviceFriends.FindUserByQRtext(qrText);
                    friendUpdate.Voter = true;
                    friendUpdate.VotingDate = DateTime.Now.Date;
                    
                }
                catch(Exception ex)
                {
                    return Ok("Пользователь не найден.");
                }

                try
                {
                    await serviceFriends.SaveFriends(friendUpdate);
                    return Ok("<h4>"+ friendUpdate.Name+" "+friendUpdate.PatronymicName + ", поздравляем, Вы зарегистрированы как участник розыгрыша!" + "</h4>");
                }
                catch(Exception ex)
                {
                    return Ok("<h4>Не удалось сохранить данные. Обратитесь к администратору!</h4>" + " "+ex.ToString());
                }
            }
            else
            {
                return Ok("<h4>Ошибка QR-кода. QR-код не содержет индификатора пользователя.</h4>");
            }
            
        }
    }
}
