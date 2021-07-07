using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using voteCollector.Data;
using voteCollector.Models;

namespace Generater.Controllers
{
    [Authorize(Roles = "admin")]
    public class PollingStationsController : Controller
    {
        private readonly ILogger<PollingStationsController> _logger;
        private readonly VoterCollectorContext _context;

        public PollingStationsController(VoterCollectorContext context, ILogger<PollingStationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: PollingStations
        public async Task<IActionResult> Index()
        {
            var voterCollectorContext = _context.PollingStation.Include(p => p.City).Include(p => p.House).Include(p => p.MicroDistrict).Include(p => p.Street);
            return View(await voterCollectorContext.ToListAsync());
        }

        // GET: PollingStations/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name");
            ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "Name");
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name");
            ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "Name");
            return View();
        }

        // POST: PollingStations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPollingStation,Name,CityId,StreetId,MicroDistrictId,HouseId")] PollingStation pollingStation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pollingStation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", pollingStation.CityId);
            ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "Name", pollingStation.HouseId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", pollingStation.MicroDistrictId);
            ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "Name", pollingStation.StreetId);
            return View(pollingStation);
        }

        // GET: PollingStations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pollingStation = await _context.PollingStation.FindAsync(id);
            if (pollingStation == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", pollingStation.CityId);
            ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "Name", pollingStation.HouseId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", pollingStation.MicroDistrictId);
            ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "Name", pollingStation.StreetId);
            return View(pollingStation);
        }

        // POST: PollingStations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPollingStation,Name,CityId,StreetId,MicroDistrictId,HouseId")] PollingStation pollingStation)
        {
            if (id != pollingStation.IdPollingStation)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pollingStation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PollingStationExists(pollingStation.IdPollingStation))
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
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", pollingStation.CityId);
            ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "Name", pollingStation.HouseId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", pollingStation.MicroDistrictId);
            ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "Name", pollingStation.StreetId);
            return View(pollingStation);
        }

        // GET: PollingStations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pollingStation = await _context.PollingStation
                .Include(p => p.City)
                .Include(p => p.House)
                .Include(p => p.MicroDistrict)
                .Include(p => p.Street)
                .FirstOrDefaultAsync(m => m.IdPollingStation == id);
            if (pollingStation == null)
            {
                return NotFound();
            }

            return View(pollingStation);
        }

        // POST: PollingStations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pollingStation = await _context.PollingStation.FindAsync(id);
            _context.PollingStation.Remove(pollingStation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PollingStationExists(int id)
        {
            return _context.PollingStation.Any(e => e.IdPollingStation == id);
        }

        [HttpGet]
        public async Task<IActionResult> RedirectTo()
        {
            return RedirectToAction("Index", "Admin");
        }
    }
}
