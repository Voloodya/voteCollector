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
            List<Groupsusers> groupsUsers = _context.Groupsusers.Include(gr => gr.GroupU).Include(gr => gr.User).Where(gr => gr.UserId == userSave.IdUser).ToList();
            return groupsUsers.Select(g => g.GroupU).ToList();
        }

        public IQueryable<Groupsusers> FilterGroupsUsers(List<Groupu> groupsUser)
        {
            Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

            if (groupsUser.Contains(mainGroup))
            {
                var voterCollectorContext = _context.Groupsusers.Include(g => g.GroupU).Include(g => g.GroupU.FieldActivity).Include(g => g.User);
                return  voterCollectorContext;
            }
            else
            {
                var voterCollectorContext = _context.Groupsusers.Include(g => g.GroupU).Include(g => g.GroupU.FieldActivity).Include(g => g.User).
                    Where(g => groupsUser.Contains(g.GroupU));
                return  voterCollectorContext;
            }
        }

        public List<Groupu> GetAllChildGroupsBFS(Groupu rootGroup, Groupu groupStart, Groupu goal)
        {
            // rootGroup - корневая группа
            // groupStart - начальный потомок
            // goal - пункт назначения, его нет

            // Список всех посещенных потомков
            List<Groupu> childGroupus = new List<Groupu>();

            // инициализируем очередь
            Queue<Groupu> queueGroupu = new Queue<Groupu>();
            // добавляем groupStart в очередь
            queueGroupu.Enqueue(groupStart);
            // помечаем groupStart как посещенную вершину во избежание повторного добавления в очередь
            //groupStart.Visited = true;

            while (queueGroupu.Count > 0)
            {
                // удаляем первый (верхний) элемент из очереди
                Groupu gr = queueGroupu.Dequeue();
                //Добавляем посещенную вершину в список потомков
                childGroupus.Add(gr);
                // rootGroup.InverseGroupParents - прямые потомки
                foreach (Groupu childgr in rootGroup.InverseGroupParents)
                {
                    // добавляем его в очередь
                    queueGroupu.Enqueue(childgr);
                    //Добавляем посещенную вершину в список потомков
                    childGroupus.Add(childgr);

                    ////если вершина ещё не посещалась
                    //if (!childgr.Visited)
                    //{
                    //    // добавляем его в очередь
                    //    queueGroupu.Enqueue(childgr);
                    //    // помечаем вершину как посещенную
                    //    childgr.Visited = true;
                    //    //Добавляем посещенную вершину в список потомков
                    //    childGroupus.Add(childgr);

                    //    if (childgr.IdGroup == goal.IdGroup) return childGroupus;
                    //}                        
                }
            }
            // Возвращение списка потомков
            return childGroupus;
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

        public User SearchUserByUserName(string userName)
        {
            return _context.User.FirstOrDefault(u => u.UserName.Equals(userName));
        }
        
    }
}
