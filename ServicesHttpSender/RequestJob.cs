using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using voteCollector.IServices;

namespace voteCollector.ModelsHttpSender
{
    // This is the background tasks that you want to run
    [DisallowConcurrentExecution]
    public class RequestJob : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IRequestSender _requestSender;
        private readonly string url = "https://etsa.online/app/api/barcodes/get.php";

        public RequestJob(IServiceScopeFactory serviceScopeFactory)
        {
            this._serviceScopeFactory = serviceScopeFactory;
        }
        public Task Execute(IJobExecutionContext context)
        {
            string dateTimeRequestStr;
            DateTime dateTimeUTC = DateTime.UtcNow;
            dateTimeRequestStr = dateTimeUTC.AddMinutes(280).ToString("yyyy-MM-dd hh:mm:ss");

            Dictionary<string, string> requestMessageDict = new Dictionary<string, string>
            {
                ["token"] = "N2U1OThkZDZkZDliZGFiM/lAfhoRNk0D+iJh2Z1h1fpYEUWXkzsbvGg1",
                ["date"] = dateTimeRequestStr
            };

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var requestSender = scope.ServiceProvider.GetService<IRequestSender>();
                return _requestSender.RequestQRcodesReceiving(url, requestMessageDict);
            }

            // отправили запрос, получили ответ, записали в БД данные ответа
            //return Task.CompletedTask;
        }
    }
}
