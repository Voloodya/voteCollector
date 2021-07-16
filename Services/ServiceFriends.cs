using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using voteCollector.Data;
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
    }
}
