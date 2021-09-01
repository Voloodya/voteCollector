using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voteCollector.DTO
{
    public class FileUploadDTO
    {
        public string NumberStrFile { get; set; }
        public string FamilyName { get; set; }
        public string Name { get; set; }
        public string PatronymicName { get; set; }
        public string DateBirth { get; set; }
        public string CityName { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string ErrorUpload { get; set; }
    }
}
