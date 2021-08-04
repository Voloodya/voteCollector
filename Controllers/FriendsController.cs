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
using voteCollector.Services;

namespace voteCollector.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class FriendsController : Controller
    {
        private readonly ILogger<FriendsController> _logger;
        private readonly VoterCollectorContext _context;
        private string NameServer;
        private string WayController;
        private string NameQRcodeParametrs;
        private ServiceUser _serviceUser;


        public FriendsController(VoterCollectorContext context, ILogger<FriendsController> logger)
        {
            _logger = logger;
            _context = context;
            NameServer = "http://195.226.209.40";
            WayController = "/CollectVoters/api/QRcodeСheckAPI/checkqrcode";
            NameQRcodeParametrs = "qrText";
            _serviceUser = new ServiceUser(context);
        }

        // GET: Friends
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userName;
            // userName=(string)TempData["userName"];
            userName = User.Identity.Name;

            var voterCollectorContext = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
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

            var voterCollectorContext = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
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
                .Include(f => f.ElectoralDistrict)
                .Include(f => f.FieldActivity)
                .Include(f => f.GroupU)
                .Include(f => f.House)
                .Include(f => f.MicroDistrict)
                .Include(f => f.Station)
                .Include(f => f.Street)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.IdFriend == id);
            if (friend == null)
            {
                return NotFound();
            }

            if (friend.Qrcode != null && !friend.Qrcode.Equals(""))
            {
                try
                {
                    friend.QRcodeBytes = QRcodeServices.BitmapToBytes(QRcodeServices.ReadingQRcodeFromFile(friend.Qrcode));
                }
                catch(Exception ex) { }
            }
            return View(friend);
        }

        // GET: Friends/Create
        [HttpGet]
        public IActionResult Create()
        {
            int selectedIndexCity = 1;

            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);

            ViewData["GroupUId"] = new SelectList(_serviceUser.FilterGroups(groupsUser), "IdGroup", "Name");
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", selectedIndexCity);
            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name");
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name");
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name");
            List<Street> selectStreets =_context.Street.Where(s => s.CityId == selectedIndexCity).ToList();
            ViewData["StreetId"] = new SelectList(selectStreets, "IdStreet", "Name");
            // ???
            IQueryable<House> selectHouse = _context.House.Where(h => h.StreetId == selectStreets[0].IdStreet);
            ViewData["HouseId"] = new SelectList(selectHouse, "IdHouse", "Name");

            List<PollingStation> polingStations = _context.PollingStation.Where(p => p.CityId == selectedIndexCity).ToList().GroupBy(p => p.Name).Select(grp => grp.FirstOrDefault()).ToList();
            //IQueryable<PollingStation> filteredStations = _context.PollingStation.Where(p => p.CityId == 1);
            //var polingStations = filteredStations.Where(p => p.IdPollingStation == filteredStations.Where(x => x.Name == p.Name).Min(y => y.IdPollingStation));
            //ViewData["PollingStationId"] = new SelectList(polingStations, "IdPollingStation", "Name");

            ///////
            int[] stationsId = polingStations.Select(p => p.StationId ?? 0).ToArray();
            List<Station> stations = _context.Station.Where(s => stationsId.Contains(s.IdStation)).ToList();
            ViewData["StationId"] = new SelectList(stations, "IdStation", "Name");
            //////

            ViewData["UserId"] = new SelectList(_context.User.Select(x => 
            new { IdUser = x.IdUser, UserName=x.UserName, Password=x.Password, RoleId=x.RoleId, FamilyName = x.FamilyName ?? string.Empty, Name=x.Name ?? string.Empty, PatronymicName=x.PatronymicName ?? string.Empty, DateBirth=x.DateBirth, Telephone=x.Telephone,
                Role=x.Role, Friends=x.Friends, Groupsusers=x.Groupsusers, numberFriends=x.numberFriends
            }), "IdUser", "FamilyName");
            return View();
        }

        // POST: Friends/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFriend,FamilyName,Name,PatronymicName,DateBirth,CityId,ElectoralDistrictId,StreetId,MicroDistrictId,HouseId,Building,Apartment,Telephone,StationId,PollingStationId,Organization,FieldActivityId,PhoneNumberResponsible,DateRegistrationSite,VotingDate,Voter,Adress,TextQRcode,Qrcode,Email,Description,UserId,GroupUId")] Friend friend)
        {
            List<Friend> searchFriend = _context.Friend.Where(frnd => frnd.Name.Equals(friend.Name) && frnd.FamilyName.Equals(friend.FamilyName) && frnd.PatronymicName.Equals(friend.PatronymicName) && frnd.DateBirth.Value.Date == friend.DateBirth.Value.Date).ToList();

            if (searchFriend.Count == 0)
            {
                if (ModelState.IsValid)
                {
                    string host = this.HttpContext.Request.Host.ToString();
                    string path = this.HttpContext.Request.Host.ToString();
                    User userSave = _context.User.Where(u => u.UserName.Equals(User.Identity.Name)).FirstOrDefault();
                    friend.UserId = userSave.IdUser;
                    //friend.GroupUId = userSave.Groupsusers.First().GroupUId;
                    string fileNameQRcode = QRcodeServices.GenerateQRcodeFile(friend.FamilyName + " " + friend.Name + " " + friend.PatronymicName, friend.DateBirth.Value.Date.ToString("d"), NameServer+WayController+'?'+NameQRcodeParametrs+'=' + friend.TextQRcode,"png");
                    friend.Qrcode = fileNameQRcode;

                    ////???
                    //PollingStation pollingStationSearch = _context.PollingStation.Where(p => p.IdPollingStation == friend.PollingStationId).FirstOrDefault();
                    //friend.StationId = pollingStationSearch.StationId;
                    ////???

                    _context.Add(friend);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);

                ViewData["GroupUId"] = new SelectList(_serviceUser.FilterGroups(groupsUser), "IdGroup", "Name", friend.GroupUId);
                ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
                ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name", friend.ElectoralDistrict);
                ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name", friend.FieldActivityId);
                ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "Name", friend.StreetId);
                ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "Name", friend.HouseId);
                ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", friend.MicroDistrictId);
                ViewData["StationId"] = new SelectList(_context.Station, "IdStation", "Name", friend.StationId);
                ViewData["UserId"] = new SelectList(_context.User, "IdUser", "FamilyName", friend.UserId);
                return View(friend);
            }
            else return Content("Данный пользователь уже был внесен в списки ранее!");
        }

        [HttpGet]
        public IActionResult GetHouses(int? id)
        {
            List<House> house = _context.House.Where(h => h.StreetId == id).ToList<House>();
            return PartialView(_context.House.Where(h => h.StreetId == id));
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

            ViewData["GroupUId"] = new SelectList(_serviceUser.FilterGroups(groupsUser), "IdGroup", "Name", friend.GroupUId);
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name", friend.ElectoralDistrictId);
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name", friend.FieldActivityId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", friend.MicroDistrictId);
            List<PollingStation> polingStations = _context.PollingStation.Where(p => p.CityId == friend.CityId).ToList().GroupBy(p => p.Name).Select(grp => grp.FirstOrDefault()).ToList();
            //ViewData["PollingStationId"] = new SelectList(polingStations, "IdPollingStation", "Name", friend.PollingStationId);

            ///////
            int[] stationsId = polingStations.Select(p => p.StationId ?? 0).ToArray();
            List<Station> stations = _context.Station.Where(s => stationsId.Contains(s.IdStation)).ToList();
            ViewData["StationId"] = new SelectList(stations, "IdStation", "Name",friend.Station);
            //////

            List<Street> selectStreets = _context.Street.Where(s => s.CityId==friend.CityId).ToList();
            ViewData["StreetId"] = new SelectList(selectStreets, "IdStreet", "Name", friend.StreetId);            
            List<House> selectHouse = _context.House.Where(h => h.StreetId == friend.StreetId).ToList();
            ViewData["HouseId"] = new SelectList(selectHouse, "IdHouse", "Name", friend.HouseId);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("IdFriend,FamilyName,Name,PatronymicName,DateBirth,CityId,ElectoralDistrictId,StreetId,MicroDistrictId,HouseId,Building,Apartment,Telephone,StationId,PollingStationId,Organization,FieldActivityId,PhoneNumberResponsible,DateRegistrationSite,VotingDate,Voter,Adress,TextQRcode,Qrcode,Email,Description,UserId,GroupUId")] Friend friend)
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
                        string fileNameQRcode = QRcodeServices.GenerateQRcodeFile(friend.FamilyName + " " + friend.Name + " " + friend.PatronymicName, friend.DateBirth.Value.Date.ToString("d"), NameServer + WayController + '?' + NameQRcodeParametrs + '=' + friend.TextQRcode, "png");
                        friend.Qrcode = fileNameQRcode;

                        ////???
                        //PollingStation pollingStationSearch = _context.PollingStation.Where(p => p.IdPollingStation == friend.PollingStationId).FirstOrDefault();
                        //friend.StationId = pollingStationSearch.StationId;
                        ////???

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

            ViewData["GroupUId"] = new SelectList(_serviceUser.FilterGroups(groupsUser), "IdGroup", "Name", friend.GroupUId);
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name", friend.ElectoralDistrictId);
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name", friend.FieldActivityId);
            ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "Name", friend.HouseId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", friend.MicroDistrictId);
            ViewData["StationId"] = new SelectList(_context.Station, "IdStation", "Name", friend.StationId);
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
                .Include(f => f.ElectoralDistrict)
                .Include(f => f.FieldActivity)
                .Include(f => f.GroupU)
                .Include(f => f.House)
                .Include(f => f.MicroDistrict)
                .Include(f => f.Station)
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
