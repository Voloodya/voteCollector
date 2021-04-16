using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using voteCollector.Data;
using voteCollector.Models;

namespace voteCollector.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class FriendsController : Controller
    {
        private readonly voterCollectorContext _context;

        public FriendsController(voterCollectorContext context)
        {
            _context = context;
        }

        // GET: Friends
        public async Task<IActionResult> Index()
        {
            var voterCollectorContext = _context.Friend.Include(f => f.City).Include(f => f.District).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.PollingStation).Include(f => f.Street).Include(f => f.User);
            return View(await voterCollectorContext.ToListAsync());
        }

        // GET: Friends/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friend = await _context.Friend
                .Include(f => f.City)
                .Include(f => f.District)
                .Include(f => f.FieldActivity)
                .Include(f => f.GroupU)
                .Include(f => f.House)
                .Include(f => f.MicroDistrict)
                .Include(f => f.PollingStation)
                .Include(f => f.Street)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.IdFriend == id);
            if (friend == null)
            {
                return NotFound();
            }

            return View(friend);
        }

        // GET: Friends/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "IdCity");
            ViewData["DistrictId"] = new SelectList(_context.District, "IdDistrict", "IdDistrict");
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "IdFieldActivity");
            ViewData["GroupUId"] = new SelectList(_context.Groupu, "IdGroup", "IdGroup");
            ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "IdHouse");
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "IdMicroDistrict");
            ViewData["PollingStationId"] = new SelectList(_context.PollingStation, "IdPollingStation", "IdPollingStation");
            ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "IdStreet");
            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "Password");
            return View();
        }

        // POST: Friends/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFriend,FamilyName,Name,PatronymicName,DateBirth,CityId,DistrictId,StreetId,MicroDistrictId,HouseId,Building,Apartment,Telephone,PollingStationId,Organization,FieldActivityId,PhoneNumberResponsible,DateRegistrationSite,VotingDate,Voter,Adress,Qrcode,Description,UserId,GroupUId")] Friend friend)
        {
            if (ModelState.IsValid)
            {
                _context.Add(friend);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "IdCity", friend.CityId);
            ViewData["DistrictId"] = new SelectList(_context.District, "IdDistrict", "IdDistrict", friend.DistrictId);
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "IdFieldActivity", friend.FieldActivityId);
            ViewData["GroupUId"] = new SelectList(_context.Groupu, "IdGroup", "IdGroup", friend.GroupUId);
            ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "IdHouse", friend.HouseId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "IdMicroDistrict", friend.MicroDistrictId);
            ViewData["PollingStationId"] = new SelectList(_context.PollingStation, "IdPollingStation", "IdPollingStation", friend.PollingStationId);
            ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "IdStreet", friend.StreetId);
            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "Password", friend.UserId);
            return View(friend);
        }

        // GET: Friends/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friend = await _context.Friend.FindAsync(id);
            if (friend == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "IdCity", friend.CityId);
            ViewData["DistrictId"] = new SelectList(_context.District, "IdDistrict", "IdDistrict", friend.DistrictId);
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "IdFieldActivity", friend.FieldActivityId);
            ViewData["GroupUId"] = new SelectList(_context.Groupu, "IdGroup", "IdGroup", friend.GroupUId);
            ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "IdHouse", friend.HouseId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "IdMicroDistrict", friend.MicroDistrictId);
            ViewData["PollingStationId"] = new SelectList(_context.PollingStation, "IdPollingStation", "IdPollingStation", friend.PollingStationId);
            ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "IdStreet", friend.StreetId);
            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "Password", friend.UserId);
            return View(friend);
        }

        // POST: Friends/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("IdFriend,FamilyName,Name,PatronymicName,DateBirth,CityId,DistrictId,StreetId,MicroDistrictId,HouseId,Building,Apartment,Telephone,PollingStationId,Organization,FieldActivityId,PhoneNumberResponsible,DateRegistrationSite,VotingDate,Voter,Adress,Qrcode,Description,UserId,GroupUId")] Friend friend)
        {
            if (id != friend.IdFriend)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(friend);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FriendExists(friend.IdFriend))
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
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "IdCity", friend.CityId);
            ViewData["DistrictId"] = new SelectList(_context.District, "IdDistrict", "IdDistrict", friend.DistrictId);
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "IdFieldActivity", friend.FieldActivityId);
            ViewData["GroupUId"] = new SelectList(_context.Groupu, "IdGroup", "IdGroup", friend.GroupUId);
            ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "IdHouse", friend.HouseId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "IdMicroDistrict", friend.MicroDistrictId);
            ViewData["PollingStationId"] = new SelectList(_context.PollingStation, "IdPollingStation", "IdPollingStation", friend.PollingStationId);
            ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "IdStreet", friend.StreetId);
            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "Password", friend.UserId);
            return View(friend);
        }

        // GET: Friends/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friend = await _context.Friend
                .Include(f => f.City)
                .Include(f => f.District)
                .Include(f => f.FieldActivity)
                .Include(f => f.GroupU)
                .Include(f => f.House)
                .Include(f => f.MicroDistrict)
                .Include(f => f.PollingStation)
                .Include(f => f.Street)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.IdFriend == id);
            if (friend == null)
            {
                return NotFound();
            }

            return View(friend);
        }

        // POST: Friends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var friend = await _context.Friend.FindAsync(id);
            _context.Friend.Remove(friend);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FriendExists(long id)
        {
            return _context.Friend.Any(e => e.IdFriend == id);
        }
    }
}
