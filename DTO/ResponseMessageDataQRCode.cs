using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voteCollector.DTO
{
    public class ResponseMessageDataQRCode
    {
        public string status { get; set; }
        public string error { get; set; }
        public int region { get; set; }
        public int amount { get; set; }
        //public string date { get; set; }
        //public Items [] items { get; set; }
        public string date { get; set; }
        

    } 
    
    public class ResponseMessageDataQRCodeSuccessfully : ResponseMessageDataQRCode
    {
        public Dictionary<string, string> items { get; set; }
    }

    public class ResponseMessageDataQRCodeError : ResponseMessageDataQRCode
    {
        public string [] items { get; set; }
    }
}
