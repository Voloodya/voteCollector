using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace voteCollector.Controllers
{
    public class RequestQRcodesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
