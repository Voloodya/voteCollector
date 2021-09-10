using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using voteCollector.Data;
using voteCollector.Models;

namespace voteCollector.Controllers
{
    public class DistrictsController : Controller
    {

        private readonly ILogger<DistrictsController> _logger;
        private readonly VoterCollectorContext _context;

        public DistrictsController(VoterCollectorContext context, ILogger<DistrictsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: DistrictsController
        //[ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        public async Task<IActionResult> Index()
        {
            var districtsContext = _context.District.Include(s => s.Station).Include(p => p.ElectoralDistrict).Include(d => d.ElectoralDistrictGovDuma)
                .Include(d => d.ElectoralDistrictAssemblyLaw);

            return View(await districtsContext.ToListAsync());
        }


        // GET: DistrictsController/Create
        public ActionResult Create()
        {
            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name");
            ViewData["ElectoralDistrictGovDumaId"] = new SelectList(_context.ElectoralDistrictGovDuma, "IdElectoralDistrictGovDuma", "Name");
            ViewData["ElectoralDistrictAssemblyLawId"] = new SelectList(_context.ElectoralDistrictAssemblyLaw, "IdElectoralDistrictAssemblyLaw", "Name");
            ViewData["StationId"] = new SelectList(_context.Station, "IdStation", "Name");

            return View();
        }

        // POST: DistrictsController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDistrict,Name,ElectoralDistrictId,ElectoralDistrictGovDumaId,ElectoralDistrictAssemblyLawId,StationId,CityId,StreetId")] District district)
        {
            if (ModelState.IsValid)
            {
                _context.Add(district);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name", district.ElectoralDistrictId);
            ViewData["ElectoralDistrictGovDumaId"] = new SelectList(_context.ElectoralDistrictGovDuma, "IdElectoralDistrictGovDuma", "Name", district.ElectoralDistrictGovDumaId);
            ViewData["ElectoralDistrictAssemblyLawId"] = new SelectList(_context.ElectoralDistrictAssemblyLaw, "IdElectoralDistrictAssemblyLaw", "Name", district.ElectoralDistrictAssemblyLawId);
            ViewData["StationId"] = new SelectList(_context.Station, "IdStation", "Name", district.StationId);

            return View(district);
        }

        // GET: DistrictsController1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.District.FindAsync(id);

            if (district == null)
            {
                return NotFound();
            }

            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name", district.ElectoralDistrictId);
            ViewData["ElectoralDistrictGovDumaId"] = new SelectList(_context.ElectoralDistrictGovDuma, "IdElectoralDistrictGovDuma", "Name", district.ElectoralDistrictGovDumaId);
            ViewData["ElectoralDistrictAssemblyLawId"] = new SelectList(_context.ElectoralDistrictAssemblyLaw, "IdElectoralDistrictAssemblyLaw", "Name", district.ElectoralDistrictAssemblyLawId);
            ViewData["StationId"] = new SelectList(_context.Station, "IdStation", "Name", district.StationId);

            return View(district);
        }

        // POST: DistrictsController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDistrict,Name,ElectoralDistrictId,ElectoralDistrictGovDumaId,ElectoralDistrictAssemblyLawId,StationId,CityId,StreetId")] District district)
        {
            if (id != district.IdDistrict)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(district);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistrictExists(district.IdDistrict))
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

            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name", district.ElectoralDistrictId);
            ViewData["ElectoralDistrictGovDumaId"] = new SelectList(_context.ElectoralDistrictGovDuma, "IdElectoralDistrictGovDuma", "Name", district.ElectoralDistrictGovDumaId);
            ViewData["ElectoralDistrictAssemblyLawId"] = new SelectList(_context.ElectoralDistrictAssemblyLaw, "IdElectoralDistrictAssemblyLaw", "Name", district.ElectoralDistrictAssemblyLawId);
            ViewData["StationId"] = new SelectList(_context.Station, "IdStation", "Name", district.StationId);

            return View(district);
        }

        // GET: DistrictsController1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.District
                .Include(d => d.ElectoralDistrict)
                .Include(d => d.Station)
                .Include(d => d.CityDistrict)
                .FirstOrDefaultAsync(m => m.IdDistrict == id);
            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        // POST: DistrictsController1/Delete/5
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

        // POST: PollingStations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pollingStation = await _context.District.FindAsync(id);
            _context.District.Remove(pollingStation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistrictExists(int id)
        {
            return _context.District.Any(e => e.IdDistrict == id);
        }

        [HttpGet]
        public async Task<IActionResult> RedirectTo()
        {
            return RedirectToAction("LkAdmin", "Admin");
        }

    }
}
