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

        public IQueryable<Groupsusers> FilterGroupsUsers(List<Groupu> groupsUser)
        {
            Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

            if (groupsUser.Contains(mainGroup))
            {
                var voterCollectorContext = _context.Groupsusers.Include(g => g.GroupU).Include(g => g.User);
                return  voterCollectorContext;
            }
            else
            {
                var voterCollectorContext = _context.Groupsusers.Include(g => g.GroupU).Include(g => g.User).
                    Where(g => groupsUser.Contains(g.GroupU));
                return  voterCollectorContext;
            }
        }

        public IQueryable<Groupu> FilterGroups(List<Groupu> groupsUser)
        {
            Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

            if (groupsUser.Contains(mainGroup))
            {
                return _context.Groupu;
            }
            else
            {
                return _context.Groupu.Where(g => groupsUser.Contains(g));
            }
        }
        
    }
}
