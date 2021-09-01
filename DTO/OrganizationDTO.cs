using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voteCollector.DTO
{
    public class OrganizationDTO
    {
        public int IdOrganization { get; set; }
        public string Name { get; set; }
        public int? LimitUpload { get; set; }
    }
}
