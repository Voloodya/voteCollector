using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using voteCollector.Data;
using voteCollector.DTO;
using voteCollector.Models;
using voteCollector.Services;

namespace CollectVoters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class APIFriendsController : ControllerBase
    {
        private readonly VoterCollectorContext _context;
        private ServiceFriends _serviceFriends;

        public APIFriendsController(VoterCollectorContext context)
        {
            _context = context;
            _serviceFriends = new ServiceFriends();
        }

        // GET: api/APIFriends
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Friend>>> GetFriend()
        {
            return await _context.Friend.ToListAsync();
        }

        // GET: api/APIFriends/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Friend>> GetFriend(long id)
        {
            var friend = await _context.Friend.FindAsync(id);

            if (friend == null)
            {
                return NotFound();
            }

            return friend;
        }

        // PUT: api/APIFriends/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFriend(long id, Friend friend)
        {
            if (id != friend.IdFriend)
            {
                return BadRequest();
            }

            _context.Entry(friend).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/APIFriends
        [HttpPost]
        public async Task<ActionResult<Friend>> PostFriend(Friend friend)
        {
            _context.Friend.Add(friend);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFriend", new { id = friend.IdFriend }, friend);
        }

        // DELETE: api/APIFriends/DeleteFriend/5
        [HttpDelete("DeleteFriend/{id}")]
        public async Task<ActionResult<Friend>> DeleteFriend(long id)
        {
            var friend = await _context.Friend.FindAsync(id);
            if (friend == null)
            {
                return NotFound();
            }

            _context.Friend.Remove(friend);
            await _context.SaveChangesAsync();

            return friend;
        }

        // POST: api/APIFriends/DeleteFriends/
        [HttpPost("DeleteFriends")]
        public async Task<IActionResult> DeleteFriends([ModelBinder] long [] idFriends)
        {
            List<Friend> friends = new List<Friend>();

            try
            {
                await Task.Run(() => _serviceFriends.RemoveFriends(_serviceFriends.SearchFriendsByIds(idFriends)));

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.ToString());
            }
            
        }

        [HttpPost("SearchFriendsByElectoralDistrict")]
        public async Task<List<FriendDTO>> SearchFriendsByElectoralDistrict([FromBody] ElectoralDistrictDTO electoralDistrictDTO)
        {
            List<Friend> friends = await _serviceFriends.SearchFriendsByElectoralDistrict(electoralDistrictDTO).ToListAsync();

            List<FriendDTO> friendDTOs = friends.Select(frnd => new FriendDTO
            {
                IdFriend = frnd.IdFriend,
                FamilyName = frnd.FamilyName,
                Name = frnd.Name,
                PatronymicName = frnd.PatronymicName,
                DateBirth = frnd.DateBirth.ToString(),
                CityName = frnd.CityDistrict.Name,
                Street = frnd.Street.Name,
                Microdistrict = frnd.MicroDistrict.Name,
                House = frnd.House.Name,
                Apartment = frnd.Apartment,
                Telephone = frnd.Telephone,
                ElectiralDistrict = frnd.ElectoralDistrict.Name,
                PollingStationName = frnd.Station.Name,
                Organization = frnd.Organization,
                FieldActivityName = frnd.FieldActivity.Name,
                PhoneNumberResponsible = frnd.PhoneNumberResponsible,
                DateRegistrationSite = frnd.DateRegistrationSite.ToString(),
                VotingDate = frnd.VotingDate.ToString(),
                Vote = frnd.Voter.ToString(),
                TextQRcode = frnd.TextQRcode,
                Email = frnd.Email,
                Description = frnd.Description,
                Group = frnd.GroupU.Name,
                UserId = frnd.UserId,
                UserName = frnd.User.UserName
            }).ToList();

            return friendDTOs;
        }


        private bool FriendExists(long id)
        {
            return _context.Friend.Any(e => e.IdFriend == id);
        }
    }
}
