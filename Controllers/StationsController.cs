using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using voteCollector.Data;
using voteCollector.Models;

namespace voteCollector.Controllers
{
    [Authorize(Roles = "admin")]
    public class StationsController : Controller
    {

        private readonly ILogger<StationsController> _logger;
        private readonly VoterCollectorContext _context;

        public StationsController(VoterCollectorContext context, ILogger<StationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: StationsController
        public async Task<IActionResult> Index()
        {
            return View(await _context.Station.ToListAsync());
        }

        // GET: StationsController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StationsController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStation,Name")] Station station)
        {            
             if (ModelState.IsValid)
                {
                    _context.Add(station);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
           
             return View();

        }

        // GET: StationsController1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            var station = await _context.Station.FindAsync(id);
            if (station == null)
            {
                return NotFound();
            }
            return View(station);
        }

        // POST: StationsController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdStation,Name")] Station station)
        {
            if (id != station.IdStation)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(station);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StationExists(station.IdStation))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(station);
        }

        // GET: StationsController1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var station = await _context.Station
                .FirstOrDefaultAsync(s => s.IdStation == id);
            if (station == null)
            {
                return NotFound();
            }

            return View(station);
        }

        // POST: Stations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var station = await _context.Station.FindAsync(id);
            _context.Station.Remove(station);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StationExists(int id)
        {
            return _context.Station.Any(e => e.IdStation == id);
        }

        [HttpGet]
        public async Task<IActionResult> RedirectTo()
        {
            return RedirectToAction("Index", "Admin");
        }

    }
}
