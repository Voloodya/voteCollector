using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using voteCollector.Data;
using voteCollector.DTO;
using voteCollector.Models;
using voteCollector.Services;

namespace voteCollector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles = "admin, user")]
    [Authorize]
    public class FileApiController : ControllerBase
    {
        private readonly ILogger<FileApiController> _logger;
        private readonly VoterCollectorContext _context;

        public FileApiController(VoterCollectorContext context, ILogger<FileApiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("uploadDataFromFile")]
        [Authorize]
        public async Task<IActionResult> UploadDataFromFile([FromBody] FriendDTO[] friendsDTO)
        {
            ServiceUser serviceUser = new ServiceUser(_context);

            List<FileUploadDTO> notUploadRecords = new List<FileUploadDTO>();

            //User userSave = _context.User.Where(u => u.UserName.Equals(User.Identity.Name)).FirstOrDefault();
            User userSave = serviceUser.SearchUserByUserName(friendsDTO[1].LoginUsers);
            List<Groupu> groupsUser = serviceUser.GetGroupsUser(User.Identity.Name);
            ServiceFriends serviceFriends = new ServiceFriends();

            //Regex regexTelephone = new Regex(@"(^[+]{0,1}[0-9]{11})");
            Regex regexTelephone = new Regex(@"^\+?\d{11}$");

            for (int i=1; i< friendsDTO.Length; i++)
            {
                try
                {
                    friendsDTO[i].UserId = userSave.IdUser;
                    //Friend newFriend = CreateFreand(friendsDTO[i]);
                    Friend newFriend = serviceFriends.CreateFreand(friendsDTO[i],regexTelephone,userSave, groupsUser, _context);
                    _context.Add(newFriend);                    
                }
                catch {

                    notUploadRecords.Add(new FileUploadDTO {
                    NumberStrFile=Convert.ToString(i+1),
                    FamilyName = friendsDTO[i].FamilyName,
                    Name = friendsDTO[i].Name,
                    PatronymicName = friendsDTO[i].PatronymicName,
                    DateBirth = friendsDTO[i].DateBirth,
                    CityName = friendsDTO[i].CityDistrict,
                    Street = friendsDTO[i].Street,
                    House = friendsDTO[i].House
                    });                
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(notUploadRecords.ToArray());
            }
            catch (Exception ex)
            {
                int i = 0;
                _logger.LogError(ex.ToString()); // .Exception(ex).Message("Ошибка записи в БД").Write();
                notUploadRecords = friendsDTO.Select(x => new FileUploadDTO
                {
                    NumberStrFile = Convert.ToString(i++),
                    FamilyName = x.FamilyName,
                    Name = x.Name,
                    PatronymicName = x.PatronymicName,
                    DateBirth = x.DateBirth,
                    CityName = x.CityDistrict,
                    Street = x.Street,
                    House = x.House
                }).ToList();
                notUploadRecords.RemoveAt(0);
                return Ok(notUploadRecords.ToArray());
            }            
        }

        [HttpPost("uploadFileQRCode")]
        public async Task<IActionResult> UploadFileQRCode()
        {
            if (HttpContext.Request.Form.Files.Count > 0)
            {
                try
                {
                    foreach (var file in HttpContext.Request.Form.Files)
                    {
                        if (file != null)
                        {
                            //Преобразовать файл в поток или байт массив


                            //Image imageFromFile=file.OpenReadStream;
                            //Bitmap bitmap = new Bitmap(imageFromFile);
                            //QRcodeServices.DecoderFromImage(bitmap);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return Ok("");
        }

    }
}
