using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voteCollector.DTO
{
    public class DateTimeDTO
    {
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("dateTo")]
        public string DateTo { get; set; }
        [JsonProperty("timeTo")]
        public string TimeTo { get; set; }
    }
}
