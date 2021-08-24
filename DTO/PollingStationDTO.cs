using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voteCollector.DTO
{
    public class PollingStationDTO
    {
        public int IdPollingStation { get; set; }
        public string Name { get; set; }
        public string CityName { get; set; }
        public string StreetName { get; set; }
        public string HouseName { get; set; }
        public int? LimitUpload { get; set; }
    }
}
