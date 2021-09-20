using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voteCollector.ModelsHttpSender
{
   // Указываем Quartz, как он должен создавать экземпляры IJob.
   // Настройка, IJobFactory которая подключается к контейнеру внедрения зависимостей ASP.NET Core (IServiceProvider):
    public class SingletonJobFactory : IJobFactory
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public SingletonJobFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var job = scope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
                return job;
            }
        }
        public void ReturnJob(IJob job){}
    }
}
