using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voteCollector.Controllers
{
    public class UploadFilesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
