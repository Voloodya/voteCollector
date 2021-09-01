﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voteCollector.DTO
{
    public class ReportQrCodeExchange
    {
        public string status { get; set; }
        public string error { get; set; }
        public int numberReceivedCodes { get; set; }
        public int numberMarkedCodes { get; set; }
        public int numberNotFound { get; set; }
        public string dateTimeRequest { get; set; }
        public List<string> notFoundQRcodes { get; set; }
    }
}
