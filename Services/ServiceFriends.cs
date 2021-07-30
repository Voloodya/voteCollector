using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using voteCollector.Data;
using voteCollector.DTO;
using voteCollector.Models;

namespace voteCollector.Services
{
    public class ServiceFriends
    {
        private readonly ILogger<ServiceFriends> _logger;
        private readonly VoterCollectorContext _context;

        public ServiceFriends()
        {
            _context = new VoterCollectorContext();
        }

        public Friend FindUserByQRtext(string qrText)
        {
            Friend friend = null;

            try
            {
                friend = _context.Friend.Where(frnd => frnd.TextQRcode.Equals(qrText)).FirstOrDefault();
            }
            catch(Exception ex)
            {

            }

            return friend;
        }

        public async Task<string> SaveFriends(Friend friend)
        {
            try
            {
                _context.Update(friend);
                await _context.SaveChangesAsync();

                return "Данные пользователя обновлены";
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return ex.ToString();
            }
        }

        public List<Friend> SearchFriendsByElectoralDistrict(ElectoralDistrictDTO electoralDistrict)
        {
            List<Friend> friends = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.PollingStation).Include(f => f.Street).Include(f => f.User)
                .Where(frnd => frnd.ElectoralDistrictId == electoralDistrict.IdElectoralDistrict).ToList();

            return friends;
        }

        public List<Friend> SearchFriendsByIds(long[] idFriends)
        {
            List<Friend> friends = new List<Friend>();
            foreach (long id in idFriends)
            {
                Friend friend = _context.Friend.Find(id);

                if (friend != null)
                {
                    friends.Add(friend);
                }
                else { friends.Add(null); }
            }
            return friends;
        }

        public void RemoveFriends(List<Friend> friends)
        {
            foreach (Friend friend in friends)
            {
                if (friend != null)
                {
                    _context.Friend.Remove(friend);
                    _context.SaveChanges();
                }
            }
        }

    }
}
