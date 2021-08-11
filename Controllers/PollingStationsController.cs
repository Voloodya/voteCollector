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
using voteCollector.DTO;
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
            List<ElectoralDistrict> electoralDistrict =  _context.ElectoralDistrict.ToList();
            electoralDistrict.Sort();
            List<District> districts = _context.District.Where(d => d.ElectoralDistrict.Name.Equals(electoralDistrict[0].Name)).ToList();
            List<int> selectStationsId = districts.Select(d => d.StationId ?? 0).ToList();

            IQueryable<PollingStation> pollingStations = _context.PollingStation.Include(p => p.Station).Include(p => p.CityDistrict).Include(p => p.Street).Include(p => p.House)
                .Where(p => selectStationsId.Contains(p.StationId ?? 0));

            return View(await pollingStations.ToListAsync());
        }

        // GET: PollingStations/Create
        public IActionResult Create()
        {
            int selectedIndexCity = 1;

            ViewData["StationId"] = new SelectList(_context.Station, "IdStation", "Name");
            ViewData["CityId"] = new SelectList(_context.CityDistrict, "IdCity", "Name", selectedIndexCity);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name");
            List<Street> selectStreets = _context.Street.Where(s => s.CityId == selectedIndexCity).ToList();
            ViewData["StreetId"] = new SelectList(selectStreets, "IdStreet", "Name");
            IQueryable<House> selectHouse = _context.House.Where(h => h.StreetId == selectStreets[0].IdStreet);
            ViewData["HouseId"] = new SelectList(selectHouse, "IdHouse", "Name");
            return View();
        }

        // POST: PollingStations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPollingStation,Name,StationId,CityId,StreetId,MicroDistrictId,HouseId")] PollingStation pollingStation)
        {
            if (ModelState.IsValid)
            {
                Station stationFind = _context.Station.Where(s => s.IdStation == pollingStation.StationId).FirstOrDefault();
                pollingStation.Name = stationFind.Name;
                _context.Add(pollingStation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StationId"] = new SelectList(_context.Station, "IdStation", "Name");
            ViewData["CityId"] = new SelectList(_context.CityDistrict, "IdCity", "Name", pollingStation.CityId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", pollingStation.MicroDistrictId);
            List<Street> selectStreets = _context.Street.Where(s => s.CityId == pollingStation.CityId).ToList();
            ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "Name", pollingStation.StreetId);
            ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "Name", pollingStation.HouseId);

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
            ViewData["StationId"] = new SelectList(_context.Station, "IdStation", "Name",pollingStation.StationId);
            ViewData["CityId"] = new SelectList(_context.CityDistrict, "IdCity", "Name", pollingStation.CityId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", pollingStation.MicroDistrictId);
            List<Street> selectStreets = _context.Street.Where(s => s.CityId == pollingStation.CityId).ToList();
            ViewData["StreetId"] = new SelectList(selectStreets, "IdStreet", "Name", pollingStation.StreetId);
            List<House> selectHouse = _context.House.Where(h => h.StreetId == pollingStation.StreetId).ToList();
            ViewData["HouseId"] = new SelectList(selectHouse, "IdHouse", "Name", pollingStation.HouseId);
            return View(pollingStation);
        }

        // POST: PollingStations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPollingStation,Name,StationId,CityId,StreetId,MicroDistrictId,HouseId")] PollingStation pollingStation)
        {
            if (id != pollingStation.IdPollingStation)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Station stationFind = _context.Station.Where(s => s.IdStation == pollingStation.StationId).FirstOrDefault();
                    pollingStation.Name = stationFind.Name;
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
            ViewData["StationId"] = new SelectList(_context.Station, "IdStation", "Name", pollingStation.StationId);
            ViewData["CityId"] = new SelectList(_context.CityDistrict, "IdCity", "Name", pollingStation.CityId);
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
                .Include(p => p.StationId)
                .Include(p => p.CityDistrict)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetPolingStationsByElectoralDistrict([FromBody] ElectoralDistrictDTO electoralDistrictDTO)
        {
            List<District> districts = _context.District.Where(d => d.ElectoralDistrictId == electoralDistrictDTO.IdElectoralDistrict).ToList();
            List<int> selectStationsId = districts.Select(d => d.StationId ?? 0).ToList();

            IQueryable<PollingStation> pollingStations = _context.PollingStation.Include(p => p.Station).Include(p => p.CityDistrict).Include(p => p.Street).Include(p => p.House)
                .Where(p => selectStationsId.Contains(p.StationId ?? 0));


            return PartialView(pollingStations);

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
