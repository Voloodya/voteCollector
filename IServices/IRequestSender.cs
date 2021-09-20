using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using voteCollector.DTO;

namespace voteCollector.IServices
{
    public interface IRequestSender
    {
        Task<ResponseMessageDataQRCode> SendRequestAsync(string url, IEnumerable<KeyValuePair<string, string>> data, string str);
        public Task<ReportQrCodeExchange> RequestQRcodesReceiving(string url, Dictionary<string, string> requestDataDict);
    }
}
