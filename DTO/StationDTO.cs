using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voteCollector.DTO
{
    public class StationDTO
    {
        public int IdStation { get; set; }
        public string Name { get; set; }
        public int? LimitUpload { get; set; }
    }
}
