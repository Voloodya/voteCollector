using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voteCollector.Controllers
{
    public class UploadFilesController : Controller
    {
        private readonly ILogger<UploadFilesController> _logger;

        public UploadFilesController(ILogger<UploadFilesController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            try{
                return View();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return View();
            }
        }
    }
}
