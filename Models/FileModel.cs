using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voteCollector.Models
{
    public class FileModel
    {
        public string nameFile { get; set; }

        public FileModel(string nameFile)
        {
            this.nameFile = nameFile;
        }
    }
}
