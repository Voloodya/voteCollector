using voteCollector.Data;
using CollectVoters.DTO;
using voteCollector.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using voteCollector.Controllers;

namespace CollectVoters.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly VoterCollectorContext _context;
        private ServiceUser _serviceUser;

        public AdminController(VoterCollectorContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
            _serviceUser = new ServiceUser(context);
        }

        // GET: Friends
        [HttpGet]
        public async Task<IActionResult> Index()
        {            
            List<Groupu> groupsUser =_serviceUser.GetGroupsUser(User.Identity.Name);
            Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

            if (groupsUser.Contains(mainGroup))
            {
                var voterCollectorContext = _context.Friend.Include(f => f.City).Include(f => f.District).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.PollingStation).Include(f => f.Street).Include(f => f.User);

                return View(await voterCollectorContext.ToListAsync());
            }
            else
            {
                var voterCollectorContext = _context.Friend.Include(f => f.City).Include(f => f.District).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.PollingStation).Include(f => f.Street).Include(f => f.User).
                    Where(f => groupsUser.Contains(f.GroupU));

                return View(await voterCollectorContext.ToListAsync());
            }            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("UserName", "Password", "ReturnUrl")] LoginModel loginViewModel)
        {
            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

            if (groupsUser.Contains(mainGroup))
            {
                var voterCollectorContext = _context.Friend.Include(f => f.City).Include(f => f.District).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.PollingStation).Include(f => f.Street).Include(f => f.User);

                return View(await voterCollectorContext.ToListAsync());
            }
            else
            {
                var voterCollectorContext = _context.Friend.Include(f => f.City).Include(f => f.District).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.PollingStation).Include(f => f.Street).Include(f => f.User).
                    Where(f => groupsUser.Contains(f.GroupU));

                return View(await voterCollectorContext.ToListAsync());
            }
        }

        // GET: Friends/Details/5
        [HttpGet]
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
        [HttpGet]
        public IActionResult Create()
        {
            int selectedIndexCity = 1;

            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

            if (groupsUser.Contains(mainGroup))
            {
                ViewData["GroupUId"] = new SelectList(_context.Groupu, "IdGroup", "Name");
            }
            else
            {
                ViewData["GroupUId"] = new SelectList(_context.Groupu.Where(g => groupsUser.Contains(g)), "IdGroup", "Name");
            }

            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", selectedIndexCity);
            ViewData["DistrictId"] = new SelectList(_context.District, "IdDistrict", "Name");
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name");
            ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "Name");
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name");
            ViewData["PollingStationId"] = new SelectList(_context.PollingStation, "IdPollingStation", "Name");
            ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "Name");
            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "FamilyName");
            return View();
        }

        // POST: Friends/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFriend,FamilyName,Name,PatronymicName,DateBirth,CityId,DistrictId,StreetId,MicroDistrictId,HouseId,Building,Apartment,Telephone,PollingStationId,Organization,FieldActivityId,PhoneNumberResponsible,DateRegistrationSite,VotingDate,Voter,Adress,Qrcode,Description,UserId,GroupUId")] Friend friend)
        {
            List<Friend> searchFriend = _context.Friend.Where(frnd => frnd.Name.Equals(friend.Name) && frnd.FamilyName.Equals(friend.FamilyName) && frnd.PatronymicName.Equals(friend.PatronymicName) && frnd.DateBirth.Value.Date == friend.DateBirth.Value.Date).ToList();

            if (searchFriend.Count == 0)
            {

                if (ModelState.IsValid)
                {
                    User userSave = _context.User.Where(u => u.UserName.Equals(User.Identity.Name)).FirstOrDefault();
                    friend.UserId = userSave.IdUser;
                    //friend.GroupUId = userSave.Groupsusers.First().GroupUId;

                    _context.Add(friend);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
                Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

                if (groupsUser.Contains(mainGroup))
                {
                    ViewData["GroupUId"] = new SelectList(_context.Groupu, "IdGroup", "Name", friend.GroupUId);
                }
                else
                {
                    ViewData["GroupUId"] = new SelectList(_context.Groupu.Where(g => groupsUser.Contains(g)), "IdGroup", "Name", friend.GroupUId);
                }

                ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
                ViewData["DistrictId"] = new SelectList(_context.District, "IdDistrict", "Name", friend.DistrictId);
                ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name", friend.FieldActivityId);
                ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "Name", friend.HouseId);
                ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", friend.MicroDistrictId);
                ViewData["PollingStationId"] = new SelectList(_context.PollingStation, "IdPollingStation", "Name", friend.PollingStationId);
                ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "Name", friend.StreetId);
                ViewData["UserId"] = new SelectList(_context.User, "IdUser", "Name", friend.UserId);
                return View(friend);
            }
            else return Content("Данный пользователь уже был внесен в списки ранее!");
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

            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

            if (groupsUser.Contains(mainGroup))
            {
                ViewData["GroupUId"] = new SelectList(_context.Groupu, "IdGroup", "Name", friend.GroupUId);
            }
            else
            {
                ViewData["GroupUId"] = new SelectList(_context.Groupu.Where(g => groupsUser.Contains(g)), "IdGroup", "Name", friend.GroupUId);
            }


            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
            ViewData["DistrictId"] = new SelectList(_context.District, "IdDistrict", "Name", friend.DistrictId);
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name", friend.FieldActivityId);
            ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "Name", friend.HouseId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", friend.MicroDistrictId);
            ViewData["PollingStationId"] = new SelectList(_context.PollingStation, "IdPollingStation", "Name", friend.PollingStationId);
            ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "Name", friend.StreetId);
            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "Name", friend.UserId);
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
                DateTime dateEmpty = new DateTime();
                if (!friend.Name.Equals("") && !friend.FamilyName.Equals("") && friend.DateBirth != dateEmpty)
                {
                    List<Friend> searchFriend = _context.Friend.Where(frnd => frnd.Name.Equals(friend.Name) && frnd.FamilyName.Equals(friend.FamilyName) && frnd.PatronymicName.Equals(friend.PatronymicName) && frnd.DateBirth.Value.Date == friend.DateBirth.Value.Date && frnd.IdFriend != friend.IdFriend).ToList();

                    if (searchFriend.Count == 0)
                    {
                        User userSave = _context.User.Where(u => u.UserName.Equals(User.Identity.Name)).FirstOrDefault();
                        friend.UserId = userSave.IdUser;
                        //friend.GroupUId = userSave.Groupsusers.First().GroupUId;

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
                    else return Content("Данный пользователь уже присутствует в списках!");
                }
                else return Content("Не все обязательные поля были заполнены!");
            }

            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

            if (groupsUser.Contains(mainGroup))
            {
                ViewData["GroupUId"] = new SelectList(_context.Groupu, "IdGroup", "Name", friend.GroupUId);
            }
            else
            {
                ViewData["GroupUId"] = new SelectList(_context.Groupu.Where(g => groupsUser.Contains(g)), "IdGroup", "Name", friend.GroupUId);
            }
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
            ViewData["DistrictId"] = new SelectList(_context.District, "IdDistrict", "Name", friend.DistrictId);
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name", friend.FieldActivityId);
            ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "Name", friend.HouseId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", friend.MicroDistrictId);
            ViewData["PollingStationId"] = new SelectList(_context.PollingStation, "IdPollingStation", "Name", friend.PollingStationId);
            ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "Name", friend.StreetId);
            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "UserName", friend.UserId);
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

        [Authorize(Roles = "admin")]
        public IActionResult About()
        {
            return Content("Вход только для администратора");
        }                
    }
}
