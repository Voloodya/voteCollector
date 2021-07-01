using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using voteCollector.Data;
using voteCollector.Models;

namespace CollectVoters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class APIFriendsController : ControllerBase
    {
        private readonly VoterCollectorContext _context;

        public APIFriendsController(VoterCollectorContext context)
        {
            _context = context;
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
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
        public async Task<ActionResult<List<Friend>>> DeleteFriends([ModelBinder] long [] idFriends)
        {
            List<Friend> friends = new List<Friend>();

            await Task.Run(() => RemoveFriends(SearchFriends(idFriends)));

            return friends;
        }

        private List<Friend> SearchFriends(long[] idFriends)
        {
            List<Friend> friends = new List<Friend>();
            foreach (long id in idFriends)
            {
                Friend friend = _context.Friend.Find(id);

                if (friend != null)
                {
                    friends.Add(friend);
                }
                else { friends.Add(null);}
            }
            return friends;
        }

        private void RemoveFriends(List<Friend> friends)
        {
            foreach(Friend friend in friends)
            {
                if (friend != null)
                {
                    _context.Friend.Remove(friend);
                    _context.SaveChanges();
                }
            }                       
        }

        private bool FriendExists(long id)
        {
            return _context.Friend.Any(e => e.IdFriend == id);
        }
    }
}
