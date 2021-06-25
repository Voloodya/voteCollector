using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using voteCollector.Data;
using voteCollector.Models;

namespace voteCollector.Controllers
{
    public class ServiceUser
    {
        private readonly VoterCollectorContext _context;

        public ServiceUser(VoterCollectorContext context)
        {
            _context = context;
        }
        public List<Groupu> GetGroupsUser(string userName)
        {
            User userSave = _context.User.Where(u => u.UserName.Equals(userName)).FirstOrDefault();
            List<Groupsusers> groupsUsers = _context.Groupsusers.Include(gr => gr.GroupU).Where(gr => gr.UserId == userSave.IdUser).ToList();
            return groupsUsers.Select(g => g.GroupU).ToList();
        }
    }
}
