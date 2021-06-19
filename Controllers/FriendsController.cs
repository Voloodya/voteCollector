using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollectVoters.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using voteCollector.Data;
using voteCollector.DTO;
using voteCollector.Models;

namespace voteCollector.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class FriendsController : Controller
    {
        private readonly ILogger<FriendsController> _logger;
        private readonly VoterCollectorContext _context;

        public FriendsController(VoterCollectorContext context, ILogger<FriendsController> logger)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Friends
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userName;
            // userName=(string)TempData["userName"];
            userName = User.Identity.Name;

            var voterCollectorContext = _context.Friend.Include(f => f.City).Include(f => f.District).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.PollingStation).Include(f => f.Street).Include(f => f.User).
                                    Where(f => f.User.UserName.Equals(userName));
            return View(await voterCollectorContext.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("UserName", "Password", "ReturnUrl")] LoginModel loginViewModel)
        {
            string userName;
            // userName=(string)TempData["userName"];
            userName = User.Identity.Name;

            var voterCollectorContext = _context.Friend.Include(f => f.City).Include(f => f.District).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.PollingStation).Include(f => f.Street).Include(f => f.User).
                                    Where(f => f.User.UserName.Equals(userName));
            return View(await voterCollectorContext.ToListAsync());
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
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", selectedIndexCity);
            ViewData["DistrictId"] = new SelectList(_context.District, "IdDistrict", "Name");
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name");
            ViewData["GroupUId"] = new SelectList(_context.Groupu, "IdGroup", "Name");
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name");           
            ViewData["StreetId"] = new SelectList(_context.Street.Where(s => s.CityId==1), "IdStreet", "Name");
            ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "Name");
            ViewData["PollingStationId"] = new SelectList(_context.PollingStation, "IdPollingStation", "Name");
            ViewData["UserId"] = new SelectList(_context.User.Select(x => 
            new { IdUser = x.IdUser, UserName=x.UserName, Password=x.Password, RoleId=x.RoleId, FamilyName = x.FamilyName ?? string.Empty, Name=x.Name ?? string.Empty, PatronymicName=x.PatronymicName ?? string.Empty, DateBirth=x.DateBirth, Telephone=x.Telephone,
                Role=x.Role, Friends=x.Friends, Groupsusers=x.Groupsusers, numberFriends=x.numberFriends
            }), "IdUser", "FamilyName");
            return View();
        }


        [HttpGet]
        public IActionResult GetHouses(int? id)
        {
            List<House> house = _context.House.Where(h => h.StreetId == id).ToList<House>();
            return PartialView(_context.House.Where(h => h.StreetId == id));
        }


        // POST: Friends/Create
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
                ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
                ViewData["DistrictId"] = new SelectList(_context.District, "IdDistrict", "Name", friend.DistrictId);
                ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name", friend.FieldActivityId);
                ViewData["GroupUId"] = new SelectList(_context.Groupu, "IdGroup", "Name", friend.GroupUId);
                ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "Name", friend.HouseId);
                ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", friend.MicroDistrictId);
                ViewData["PollingStationId"] = new SelectList(_context.PollingStation, "IdPollingStation", "Name", friend.PollingStationId);
                ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "Name", friend.StreetId);
                ViewData["UserId"] = new SelectList(_context.User, "IdUser", "FamilyName", friend.UserId);
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
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
            ViewData["DistrictId"] = new SelectList(_context.District, "IdDistrict", "Name", friend.DistrictId);
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name", friend.FieldActivityId);
            ViewData["GroupUId"] = new SelectList(_context.Groupu, "IdGroup", "Name", friend.GroupUId);
            ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "Name", friend.HouseId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", friend.MicroDistrictId);
            ViewData["PollingStationId"] = new SelectList(_context.PollingStation, "IdPollingStation", "Name", friend.PollingStationId);
            var selectLists= new SelectList(_context.Street, "IdStreet", "Name", friend.StreetId);
            ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "Name", friend.StreetId);
            ViewData["UserId"] = new SelectList(_context.User.Select(x =>
            new User {
                IdUser = x.IdUser,
                UserName = x.UserName,
                Password = x.Password,
                RoleId = x.RoleId,
                FamilyName = x.FamilyName == null ? string.Empty : x.FamilyName,
                Name = x.Name ?? string.Empty,
                PatronymicName = x.PatronymicName==null ? string.Empty : x.PatronymicName,
                DateBirth = x.DateBirth,
                Telephone = x.Telephone,
                Role = x.Role,
                Friends = x.Friends,
                Groupsusers = x.Groupsusers,
                numberFriends = x.numberFriends
            }), "IdUser", "FamilyName", friend.UserId);
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
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
            ViewData["DistrictId"] = new SelectList(_context.District, "IdDistrict", "Name", friend.DistrictId);
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name", friend.FieldActivityId);
            ViewData["GroupUId"] = new SelectList(_context.Groupu, "IdGroup", "Name", friend.GroupUId);
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

        private List<Groupu> GetGroupsUser(string userName)
        {
            User userSave = _context.User.Where(u => u.UserName.Equals(userName)).FirstOrDefault();
            List<Groupsusers> groupsUsers = userSave.Groupsusers.ToList();
            return groupsUsers.Select(g => g.GroupU).ToList();
        }
    }
}
