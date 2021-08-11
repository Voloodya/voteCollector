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

            User userSave = _context.User.Where(u => u.UserName.Equals(User.Identity.Name)).FirstOrDefault();
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
                    CityName = friendsDTO[i].CityName,
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
                    CityName = x.CityName,
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

        public Friend CreateFreand(FriendDTO friendDTO)
        {
            Friend newFriend= new Friend();

            ServiceUser serviceUser = new ServiceUser(_context);

            Regex regexTelephone = new Regex(@"[+]?[0-9]{11}");

            newFriend.UserId = friendDTO.UserId;
            DateTime datesBirth;

            string[] dates = friendDTO.DateBirth.Trim().Split('.');
            try
            {
                datesBirth = new DateTime(Convert.ToInt32(dates[2]), Convert.ToInt32(dates[1]), Convert.ToInt32(dates[0]));
            }
            catch
            {
                datesBirth = new DateTime();
            }

            List<Friend> searchFriend = _context.Friend.Where(frnd => frnd.Name.Equals(friendDTO.Name.Trim()) && frnd.FamilyName.Equals(friendDTO.FamilyName.Trim()) && frnd.PatronymicName.Equals(friendDTO.PatronymicName.Trim()) && frnd.DateBirth.Value.Date == datesBirth).ToList();

            if (searchFriend.Count == 0) {

                newFriend.FamilyName = friendDTO.FamilyName.Trim();
                newFriend.Name = friendDTO.Name.Trim();
                newFriend.PatronymicName = friendDTO.PatronymicName.Trim();
                newFriend.DateBirth = datesBirth;

                if (friendDTO.CityName != null && !friendDTO.CityName.Trim().Equals(""))
                {
                    int cityDistrictId = _context.CityDistrict.Where(c => c.Name.Equals(friendDTO.CityName.Trim())).FirstOrDefault().IdCityDistrict;
                    newFriend.CityDistrictId = cityDistrictId;

                    int streetId = _context.Street.Where(s => s.Name.Equals(friendDTO.Street.Trim())).FirstOrDefault().IdStreet;
                    newFriend.StreetId = streetId;
                    if (friendDTO.Microdistrict != null && !friendDTO.Microdistrict.Trim().Equals(""))
                    {
                        newFriend.MicroDistrictId = _context.Microdistrict.Where(md => md.Name.Equals(friendDTO.Microdistrict.Trim())).FirstOrDefault().IdMicroDistrict;
                    }
                    House house = _context.House.Where(h => h.Name.Equals(friendDTO.House.Trim())).FirstOrDefault();

                    if (house != null)
                    {
                        newFriend.HouseId = house.IdHouse;
                        newFriend.Apartment = friendDTO.Apartment.Trim();

                        PollingStation pollingStation = _context.PollingStation.Where(p => (p.CityId == cityDistrictId && p.StreetId == streetId && p.HouseId == house.IdHouse)).FirstOrDefault();
                        if (pollingStation != null)
                        {
                            newFriend.StationId = pollingStation.StationId;
                        }
                        else if (friendDTO.PollingStationName != null && !friendDTO.PollingStationName.Trim().Equals(""))
                        {
                            PollingStation pollingStationSearch = _context.PollingStation.Where(p => p.Name.Equals(friendDTO.PollingStationName.Trim())).FirstOrDefault();
                            //newFriend.PollingStationId = pollingStationSearch.IdPollingStation;
                            newFriend.StationId = pollingStationSearch.StationId;
                        }
                        if (friendDTO.ElectiralDistrict != null && !friendDTO.ElectiralDistrict.Trim().Equals(""))
                        {
                            newFriend.ElectoralDistrictId = _context.ElectoralDistrict.Where(d => d.Name.Equals(friendDTO.ElectiralDistrict.Trim())).FirstOrDefault().IdElectoralDistrict;
                        }
                        else
                        {
                            District district = _context.District.Where(d => d.StationId == newFriend.StationId).FirstOrDefault();
                            newFriend.ElectoralDistrictId = district.ElectoralDistrictId;
                        }
                    }

                }

                string telephone = friendDTO.Telephone.Trim().Trim('-');
                if (regexTelephone.IsMatch(telephone))
                {
                    newFriend.Telephone = telephone;
                }
                string telephoneResponsible = friendDTO.PhoneNumberResponsible.Trim().Trim('-');
                if (regexTelephone.IsMatch(telephoneResponsible))
                {
                    newFriend.PhoneNumberResponsible = telephoneResponsible;
                }

                if (friendDTO.Email != null)
                {
                    newFriend.Email = friendDTO.Email.Trim();
                }
                if (friendDTO.TextQRcode != null)
                {
                    newFriend.TextQRcode = friendDTO.TextQRcode.Trim();
                }
                newFriend.Organization = friendDTO.Organization.Trim();
                if (friendDTO.FieldActivityName != null && !friendDTO.FieldActivityName.Trim().Equals(""))
                {
                    newFriend.FieldActivityId = _context.Fieldactivity.Where(f => f.Name.Equals(friendDTO.FieldActivityName.Trim())).FirstOrDefault().IdFieldActivity;
                }
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

                if (friendDTO.Group != null && !friendDTO.Group.Trim().Equals(""))
                {
                    newFriend.GroupUId = _context.Groupu.Where(g => g.Name.Equals(friendDTO.Group.Trim())).FirstOrDefault().IdGroup;
                }
                else
                {
                    List<Groupu> groupsUser = serviceUser.GetGroupsUser(User.Identity.Name);
                    newFriend.GroupUId = groupsUser[0].IdGroup;
                }
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
