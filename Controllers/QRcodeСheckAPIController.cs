using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using voteCollector.DTO;
using voteCollector.Models;
using voteCollector.Services;

namespace voteCollector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    //[Authorize(Roles = "admin, user")]
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

        // "/api/QRcodeСheckAPI/checphonenumber?phoneNumber="
        [HttpGet]
        [Route("checkphonenumber")]
        public async Task<IActionResult> CheckPhoneNumber(String phoneNumber)
        {
            if (phoneNumber != null && phoneNumber != "")
            {
                ServiceFriends serviceFriends = new ServiceFriends();

                string clearPhoneNumber = ServicePhoneNumber.LeaveOnlyNumbers(phoneNumber).Substring(1,10);
                Friend friendUpdate = null;
                try
                {
                    friendUpdate = serviceFriends.FindUserByPhoneNumber(clearPhoneNumber);
                    friendUpdate.Voter = true;
                    friendUpdate.VotingDate = DateTime.Now.Date;

                }
                catch (Exception ex)
                {
                    return Ok("Пользователь не найден.");
                }

                try
                {
                    await serviceFriends.SaveFriends(friendUpdate);
                    return Ok("<h4>" + friendUpdate.Name + " " + friendUpdate.PatronymicName + ", поздравляем, Вы зарегистрированы как участник розыгрыша!" + "</h4>");
                }
                catch (Exception ex)
                {
                    return Ok("<h4>Не удалось сохранить данные. Обратитесь к администратору!</h4>" + " " + ex.ToString());
                }
            }
            else
            {
                return Ok("<h4>Ошибка. Запрос не содержет номера телефона.</h4>");
            }
        }

        //"/api/QRcodeСheckAPI/checkmasjsonqrcode"
        [HttpPost]
        [Route("checkmasjsonqrcode")]
        public void CheckMasjsonqrcode(string[] qrCodeDTOs)
        {
            if (qrCodeDTOs.Length > 0)
            {
                ServiceFriends serviceFriends = new ServiceFriends();

                foreach (string qrcode in qrCodeDTOs)
                {
                    if (qrcode != null && qrcode != "")
                    {
                        Friend friendUpdate;
                        try
                        {
                            friendUpdate = serviceFriends.FindUserByQRtext(qrcode);
                            friendUpdate.Voter = true;
                            friendUpdate.VotingDate = DateTime.Now.Date;
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }

                        try
                        {
                            serviceFriends.SaveFriends(friendUpdate);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
            }
        }

        //"/api/QRcodeСheckAPI/checkmasjsonphonenumber"
        [HttpPost]
        [Route("checkmasjsonphonenumber")]
        public void CheckMasjsonPhonenumber(string[] phoneNumberDTOs)
        {

            if (phoneNumberDTOs.Length > 0)
            {
                ServiceFriends serviceFriends = new ServiceFriends();

                foreach (string phoneNumber in phoneNumberDTOs) {

                    string clearPhoneNumber = ServicePhoneNumber.LeaveOnlyNumbers(phoneNumber).Substring(1, 10);
                    Friend friendUpdate;
                    try
                    {
                        friendUpdate = serviceFriends.FindUserByPhoneNumber(clearPhoneNumber);
                        friendUpdate.Voter = true;
                        friendUpdate.VotingDate = DateTime.Now.Date;

                    }
                    catch (Exception ex)
                    {
                        continue;
                    }

                    try
                    {
                        serviceFriends.SaveFriends(friendUpdate);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }      
            }
            else
            {

            }
        }
    }
}
