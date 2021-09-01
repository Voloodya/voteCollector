using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using voteCollector.Models;

namespace voteCollector.DTO
{
    public class FileExcel
    {
        public string NameFile { get; set; }
        public Friend []  friends { get; set; }
    }
}
