using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace voteCollector.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class QRcodeController : Controller
    {
        // GET: QRcodeController
        public ActionResult Index()
        {
            return View();
        }


        // GET: QRcodeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QRcodeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QRcodeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: QRcodeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QRcodeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: QRcodeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
