using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace voteCollector.Models
{
    public class QRcode
    {
        public Image FileImageQR { get; set; }
        public string Name { get; set; }

        [BindProperty]
        public BufferedMultipleFileUploadPhysical FileUpload { get; set; }

    }

    public class BufferedMultipleFileUploadPhysical
    {
        [Required]
        [Display(Name = "File")]
        public List<IFormFile> FormFiles { get; set; }

        [Display(Name = "Note")]
        [StringLength(100, MinimumLength = 0)]
        public string Note { get; set; }
    }
}
