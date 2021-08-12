using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voteCollector.Models
{
    public class Report
    {
        public string FieldActivity { get; set; }
        public List<Organization> Organizations { get; set; }
    }
}
