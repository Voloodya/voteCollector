using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using voteCollector.Data;
using voteCollector.Models;

namespace voteCollector.Services
{
    public class ServiceGroup
    {
        private readonly VoterCollectorContext _context;

        public ServiceGroup(VoterCollectorContext context)
        {
            _context = context;
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
            queueGroupu.Enqueue(rootGroup);
            // помечаем groupStart как посещенную вершину во избежание повторного добавления в очередь
            //groupStart.Visited = true;

            while (queueGroupu.Count > 0)
            {
                // удаляем первый (верхний) элемент из очереди
                Groupu directChildgr = queueGroupu.Dequeue();
                //Добавляем посещенную вершину в список потомков
                childGroupus.Add(directChildgr);
                // rootGroup.InverseGroupParents - прямые потомки
                foreach (Groupu childgr in directChildgr.InverseGroupParents)
                {
                    _context.Entry(childgr).Collection(g => g.InverseGroupParents).Load();
                    // добавляем его в очередь
                    queueGroupu.Enqueue(childgr);
                    //Добавляем посещенную вершину в список потомков
                    //childGroupus.Add(childgr);

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
    }
}
