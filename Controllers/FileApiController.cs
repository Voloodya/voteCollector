using CollectVoters.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using voteCollector.Data;
using voteCollector.Models;

namespace voteCollector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FileApiController : ControllerBase
    {
        private readonly VoterCollectorContext _context;

        public FileApiController(VoterCollectorContext context)
        {
            _context = context;
        }

        [HttpPost("uploadDataFromFile")]
        public ActionResult UploadDataFromFile([FromBody] FriendDTO[] friendsDTO)
        {
            List<int> notUploadRecords = new List<int>();

            for(int i=0; i< friendsDTO.Length; i++)
            {
                try
                {
                    _context.Add(CreateFreand(friendsDTO[i]));                    
                }
                catch {

                    notUploadRecords.Add(i + 1);                
                }
            }
            _context.SaveChangesAsync();

            return Ok(notUploadRecords.ToArray());

        }

        public Friend CreateFreand(FriendDTO friendDTO)
        {
            Friend newFriend = new Friend();

            User userSave = _context.User.Where(u => u.UserName.Equals(User.Identity.Name)).FirstOrDefault();
            newFriend.UserId = userSave.IdUser;

            newFriend.FamilyName = friendDTO.FamilyName.Trim();
            newFriend.Name = friendDTO.Name.Trim();
            newFriend.PatronymicName = friendDTO.PatronymicName.Trim();
            //newFriend.DateBirth = friendDTO.DateBirth.Trim();
            newFriend.CityId = _context.City.Where(c => c.Name.Equals(friendDTO.CityName.Trim())).FirstOrDefault().IdCity;
            newFriend.DistrictId = _context.District.Where(d => d.Name.Equals(friendDTO.DistrictName.Trim())).FirstOrDefault().IdDistrict;
            newFriend.StreetId = _context.Street.Where(s => s.Name.Equals(friendDTO.Street.Trim())).FirstOrDefault().IdStreet;
            newFriend.MicroDistrictId = _context.Microdistrict.Where(md => md.Name.Equals(friendDTO.Microdistrict.Trim())).FirstOrDefault().IdMicroDistrict;
            newFriend.HouseId = _context.House.Where(h => h.Name.Equals(friendDTO.House.Trim())).FirstOrDefault().IdHouse;
            newFriend.Apartment = friendDTO.Apartment.Trim();
            newFriend.Telephone = friendDTO.Telephone.Trim();
            newFriend.PollingStationId = _context.PollingStation.Where(p => p.Name.Equals(friendDTO.PollingStationName.Trim())).FirstOrDefault().IdPollingStation;
            newFriend.Organization = friendDTO.Organization.Trim();
            newFriend.FieldActivityId = _context.Fieldactivity.Where(f => f.Name.Equals(friendDTO.FieldActivityName.Trim())).FirstOrDefault().IdFieldActivity;
            newFriend.PhoneNumberResponsible = friendDTO.PhoneNumberResponsible.Trim();
            //newFriend.DateRegistrationSite = friendDTO.DateRegistrationSite.Trim();
            //newFriend.VotingDate = friendDTO.VotingDate.Trim();
            newFriend.Description = friendDTO.Description;
            newFriend.GroupUId = _context.Groupu.Where(g => g.Name.Equals(friendDTO.Group.Trim())).FirstOrDefault().IdGroup;

            newFriend.Voter = friendDTO.Vote.ToLower().Trim().Equals("да") ? true : false;

            return newFriend;
        }
        
    }
}
