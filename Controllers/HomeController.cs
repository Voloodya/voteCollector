using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using voteCollector.Models;

namespace CollectVoters.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Index([Bind("UserName", "Password", "ReturnUrl")] LoginModel loginModel)
        //{
        //    var db_usersContext = _context.User.Where(u => u.UserName.Equals(loginModel.UserName) & u.Password.Equals(loginModel.Password));
        //    User userFind = null;
        //    if (db_usersContext.Count() > 0)
        //    {
        //        userFind = db_usersContext.First();
        //        return RedirectToAction("Index", "Friends", new { login = userFind.UserName});
        //    }
        //    else
        //    {
        //        TempData["msg"] = "Не верный логин или пароль!";
        //        return NotFound();
        //    }
        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
