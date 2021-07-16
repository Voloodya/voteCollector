using CollectVoters.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using voteCollector.Data;
using voteCollector.Models;
using voteCollector.Services;

namespace voteCollector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles = "admin, user")]
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
        public async Task<IActionResult> UploadDataFromFile([FromBody] FriendDTO[] friendsDTO)
        {
            List<int> notUploadRecords = new List<int>();

            User userSave = _context.User.Where(u => u.UserName.Equals(User.Identity.Name)).FirstOrDefault();

            for (int i=1; i< friendsDTO.Length; i++)
            {
                try
                {
                    friendsDTO[i].UserId = userSave.IdUser;
                    Friend newFriend = CreateFreand(friendsDTO[i]);
                    _context.Add(newFriend);
                    
                }
                catch {

                    notUploadRecords.Add(i + 1);                
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(notUploadRecords.ToArray());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString()); // .Exception(ex).Message("Ошибка записи в БД").Write();
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

        public Friend CreateFreand(FriendDTO friendDTO)
        {
            Friend newFriend= new Friend();

            newFriend.UserId = friendDTO.UserId;

            string[] dates = friendDTO.DateBirth.Trim().Split('.');
            DateTime datesBirth = new DateTime(Convert.ToInt32(dates[2]), Convert.ToInt32(dates[1]), Convert.ToInt32(dates[0]));

            List<Friend> searchFriend = _context.Friend.Where(frnd => frnd.Name.Equals(friendDTO.Name.Trim()) && frnd.FamilyName.Equals(friendDTO.FamilyName.Trim()) && frnd.PatronymicName.Equals(friendDTO.PatronymicName.Trim()) && frnd.DateBirth.Value.Date == datesBirth).ToList();

            if (searchFriend.Count == 0) {

                newFriend.FamilyName = friendDTO.FamilyName.Trim();
                newFriend.Name = friendDTO.Name.Trim();
                newFriend.PatronymicName = friendDTO.PatronymicName.Trim();
                newFriend.DateBirth = datesBirth;
                int cityId = _context.City.Where(c => c.Name.Equals(friendDTO.CityName.Trim())).FirstOrDefault().IdCity;
                newFriend.CityId = cityId;
                if (friendDTO.DistrictName != null && !friendDTO.DistrictName.Trim().Equals("")) {
                    newFriend.DistrictId = _context.District.Where(d => d.Name.Equals(friendDTO.DistrictName.Trim())).FirstOrDefault().IdDistrict;
                }
                int streetId = _context.Street.Where(s => s.Name.Equals(friendDTO.Street.Trim())).FirstOrDefault().IdStreet;
                newFriend.StreetId = streetId;
                if (friendDTO.Microdistrict != null && !friendDTO.Microdistrict.Trim().Equals("")) {
                    newFriend.MicroDistrictId = _context.Microdistrict.Where(md => md.Name.Equals(friendDTO.Microdistrict.Trim())).FirstOrDefault().IdMicroDistrict;
                }
                int houseId = _context.House.Where(h => h.Name.Equals(friendDTO.House.Trim())).FirstOrDefault().IdHouse;
                newFriend.HouseId = houseId;
                newFriend.Apartment = friendDTO.Apartment.Trim();
                newFriend.Telephone = friendDTO.Telephone.Trim();
                if (friendDTO.Email != null)
                {
                    newFriend.Email = friendDTO.Email.Trim();
                }
                if (friendDTO.TextQRcode != null)
                {
                    newFriend.TextQRcode = friendDTO.TextQRcode.Trim();
                }
                PollingStation pollingStation = _context.PollingStation.Where(p => (p.CityId == cityId && p.StreetId == streetId && p.HouseId == houseId)).FirstOrDefault();
                if (pollingStation != null)
                {
                    newFriend.PollingStationId = pollingStation.IdPollingStation;
                }
                else if (friendDTO.PollingStationName != null && !friendDTO.PollingStationName.Trim().Equals(""))
                {
                    newFriend.PollingStationId = _context.PollingStation.Where(p => p.Name.Equals(friendDTO.PollingStationName.Trim())).FirstOrDefault().IdPollingStation;
                }
                newFriend.Organization = friendDTO.Organization.Trim();
                newFriend.FieldActivityId = _context.Fieldactivity.Where(f => f.Name.Equals(friendDTO.FieldActivityName.Trim())).FirstOrDefault().IdFieldActivity;
                newFriend.PhoneNumberResponsible = friendDTO.PhoneNumberResponsible.Trim();
                if (friendDTO.DateBirth != null && !friendDTO.DateBirth.Trim().Equals(""))
                {
                    string[] datesR = friendDTO.DateRegistrationSite.Trim().Split('.');
                    DateTime datesRegistration = new DateTime(Convert.ToInt32(datesR[2]), Convert.ToInt32(datesR[1]), Convert.ToInt32(datesR[0]));
                    newFriend.DateRegistrationSite = datesRegistration;
                }
                if (friendDTO.VotingDate != null && !friendDTO.VotingDate.Trim().Equals(""))
                {
                    string[] datesV = friendDTO.VotingDate.Trim().Split('.');
                    DateTime datesVoting = new DateTime(Convert.ToInt32(datesV[2]), Convert.ToInt32(datesV[1]), Convert.ToInt32(datesV[0]));
                    newFriend.VotingDate = datesVoting;
                }
                newFriend.Description = friendDTO.Description;
                newFriend.GroupUId = _context.Groupu.Where(g => g.Name.Equals(friendDTO.Group.Trim())).FirstOrDefault().IdGroup;

                if (friendDTO.Vote != null && !friendDTO.Vote.Trim().Equals(""))
                {
                    newFriend.Voter = friendDTO.Vote.ToLower().Trim().Equals("да") ? true : false;
                }
            }
            else
            {
                throw new Exception("Пользователь уже есть в списках!");
            }

            return newFriend;
        }
        
    }
}
