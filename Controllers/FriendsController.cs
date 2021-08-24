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
        private string WayPathQrCodes;

        public FriendsController(VoterCollectorContext context, ILogger<FriendsController> logger)
        {
            _logger = logger;
            _context = context;
            NameServer = "http://оренбургвсе.рф";
            //WayController = "/CollectVoters/api/QRcodeСheckAPI/checkqrcode";
            WayController = "/api/QRcodeСheckAPI/checkqrcode";
            NameQRcodeParametrs = "qrText";
            _serviceUser = new ServiceUser(context);
            WayPathQrCodes = "wwwroot/qr_codes/";

        }

        // GET: Friends
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string userName;
            // userName=(string)TempData["userName"];
            userName = User.Identity.Name;

           List<Friend>  friends = await _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                                    Include(f => f.FriendStatus).Include(f => f.Organization_).Where(f => f.User.UserName.Equals(userName)).ToListAsync();
            return View(friends);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("UserName", "Password", "ReturnUrl")] LoginModel loginViewModel)
        {
            string userName;
            // userName=(string)TempData["userName"];
            userName = User.Identity.Name;

            var voterCollectorContext = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.Station).Include(f => f.Street).Include(f => f.User).
                Include(f => f.FriendStatus).Include(f => f.Organization_).Where(f => f.User.UserName.Equals(userName));
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
                .Include(f => f.CityDistrict)
                .Include(f => f.ElectoralDistrict)
                .Include(f => f.FieldActivity)
                .Include(f => f.GroupU)
                .Include(f => f.House)
                .Include(f => f.MicroDistrict)
                .Include(f => f.Station)
                .Include(f => f.Street)
                .Include(f => f.User)
                .Include(f => f.FriendStatus)
                .Include(f => f.Organization_)
                .FirstOrDefaultAsync(m => m.IdFriend == id);
            if (friend == null)
            {
                return NotFound();
            }

            if (friend.ByteQrcode != null)
            {
                try
                {
                    friend.QRcodeBytes = QRcodeServices.BitmapToBytes(QRcodeServices.CreateBitmapFromBytes(friend.ByteQrcode));
                }
                catch(Exception ex) { }
            }
            return View(friend);
        }

        // GET: Friends/Create
        [HttpGet]
        public IActionResult Create()
        {
            int selectedIndexCityDistrict = 1;
            int selectIndexCity = 1;

            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            int[] idFieldActivityUser = groupsUser.Select(g => g.FieldActivityId ?? 0).ToArray();
            int[] idOrganizationUser = groupsUser.Select(g => g.OrganizationId ?? 0).ToArray();

            ViewData["GroupUId"] = new SelectList(groupsUser, "IdGroup", "Name");
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", selectIndexCity);
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict, "IdCityDistrict", "Name", selectedIndexCityDistrict);
            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name");
            List<Fieldactivity> fieldactivitiesSelect = _context.Fieldactivity.Where(fac => idFieldActivityUser.Contains(fac.IdFieldActivity)).ToList();
            ViewData["FieldActivityId"] = new SelectList(fieldactivitiesSelect, "IdFieldActivity", "Name");
            List<Organization> organizationSelect = _context.Organization.Where(org => idOrganizationUser.Contains(org.IdOrganization)).ToList();
            ViewData["OrganizationId"] = new SelectList(organizationSelect, "IdOrganization", "Name");
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name");
            List<Street> selectStreets =_context.Street.Where(s => s.CityId == selectedIndexCityDistrict).ToList();
            ViewData["StreetId"] = new SelectList(selectStreets, "IdStreet", "Name");
            // ???
            IQueryable<House> selectHouse = _context.House.Where(h => h.StreetId == selectStreets[0].IdStreet);
            ViewData["HouseId"] = new SelectList(selectHouse, "IdHouse", "Name");

            List<PollingStation> polingStations = _context.PollingStation.Where(p => p.CityDistrictId == selectedIndexCityDistrict).ToList().GroupBy(p => p.Name).Select(grp => grp.FirstOrDefault()).ToList();
            //IQueryable<PollingStation> filteredStations = _context.PollingStation.Where(p => p.CityId == 1);
            //var polingStations = filteredStations.Where(p => p.IdPollingStation == filteredStations.Where(x => x.Name == p.Name).Min(y => y.IdPollingStation));
            //ViewData["PollingStationId"] = new SelectList(polingStations, "IdPollingStation", "Name");

            ///////
            int[] stationsId = polingStations.Select(p => p.StationId ?? 0).ToArray();
            List<Station> stations = _context.Station.Where(s => stationsId.Contains(s.IdStation)).ToList();
            stations.Sort((s1, s2) => Convert.ToInt32(s1.Name)- Convert.ToInt32(s2.Name));
            ViewData["StationId"] = new SelectList(stations, "IdStation", "Name");
            //////

            ViewData["UserId"] = new SelectList(_context.User.Select(x => 
            new { IdUser = x.IdUser, UserName=x.UserName, Password=x.Password, RoleId=x.RoleId, FamilyName = x.FamilyName ?? string.Empty, Name=x.Name ?? string.Empty, PatronymicName=x.PatronymicName ?? string.Empty, DateBirth=x.DateBirth, Telephone=x.Telephone,
                Role=x.Role, Friends=x.Friends, Groupsusers=x.Groupsusers, numberFriends=x.numberFriends
            }), "IdUser", "FamilyName");
            ViewData["FriendStatusId"] = new SelectList(_context.FriendStatus, "IdFriendStatus", "Name");
            return View();
        }

        // POST: Friends/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFriend,FamilyName,Name,PatronymicName,DateBirth,Unpinning,CityId,CityDistrictId,ElectoralDistrictId,StreetId,MicroDistrictId,HouseId,Building,Apartment,Telephone,StationId,PollingStationId,Organization,FieldActivityId,PhoneNumberResponsible,DateRegistrationSite,VotingDate,Voter,Adress,TextQRcode,Qrcode,Email,Description,UserId,GroupUId,FriendStatusId,OrganizationId")] Friend friend)
        {
            if (ModelState.IsValid)
            {

                List<Friend> searchFriend = _context.Friend.Where(frnd => frnd.Name.Equals(friend.Name) && frnd.FamilyName.Equals(friend.FamilyName) && frnd.PatronymicName.Equals(friend.PatronymicName) && frnd.DateBirth.Value.Date == friend.DateBirth.Value.Date).ToList();

                if (searchFriend.Count == 0)
                {
                    friend.DateRegistrationSite = DateTime.Today;

                    //string host = this.HttpContext.Request.Host.ToString();
                    //string path = this.HttpContext.Request.Host.ToString();
                    User userSave = _context.User.Where(u => u.UserName.Equals(User.Identity.Name)).FirstOrDefault();
                    friend.UserId = userSave.IdUser;
                    friend.PhoneNumberResponsible = userSave.Telephone;
                    //friend.GroupUId = userSave.Groupsusers.First().GroupUId;
                    friend.ByteQrcode = QRcodeServices.GenerateQRcodeFile(friend.FamilyName + " " + friend.Name + " " + friend.PatronymicName, friend.DateBirth.Value.Date.ToString("d"), NameServer + WayController + '?' + NameQRcodeParametrs + '=' + friend.TextQRcode, "png", WayPathQrCodes);
                    //friend.Qrcode = fileNameQRcode;
                    friend.Telephone = ServicePhoneNumber.LeaveOnlyNumbers(friend.Telephone);

                    if (friend.Unpinning && friend.CityId != 1)
                    {
                        friend.CityDistrictId = null;
                        friend.StreetId = null;
                        friend.HouseId = null;
                        friend.Apartment = null;
                        ////???
                        //PollingStation pollingStationSearch = _context.PollingStation.Where(p => p.IdPollingStation == friend.PollingStationId).FirstOrDefault();
                        //friend.StationId = pollingStationSearch.StationId;
                        ////???
                    }
                    else
                    {
                        friend.Adress = null;
                    }

                    _context.Add(friend);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else ModelState.AddModelError("", "Данный избиратель уже был внесен в списки ранее!");
            }

            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            int[] idFieldActivityUser = groupsUser.Select(g => g.FieldActivityId ?? 0).ToArray();
            int[] idOrganizationUser = groupsUser.Select(g => g.OrganizationId ?? 0).ToArray();

            ViewData["GroupUId"] = new SelectList(groupsUser, "IdGroup", "Name", friend.GroupUId);
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict, "IdCityDistrict", "Name", friend.CityDistrictId);
            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name", friend.ElectoralDistrictId);
            List<Fieldactivity> fieldactivitiesSelect = _context.Fieldactivity.Where(fac => idFieldActivityUser.Contains(fac.IdFieldActivity)).ToList();
            ViewData["FieldActivityId"] = new SelectList(fieldactivitiesSelect, "IdFieldActivity", "Name", friend.FieldActivityId);
            List<Organization> organizationSelect = _context.Organization.Where(org => idOrganizationUser.Contains(org.IdOrganization)).ToList();
            ViewData["OrganizationId"] = new SelectList(organizationSelect, "IdOrganization", "Name", friend.OrganizationId);
            ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "Name", friend.StreetId);
            ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "Name", friend.HouseId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", friend.MicroDistrictId);
            ViewData["StationId"] = new SelectList(_context.Station, "IdStation", "Name", friend.StationId);
            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "FamilyName", friend.UserId);
            ViewData["FriendStatusId"] = new SelectList(_context.FriendStatus, "IdFriendStatus", "Name", friend.FriendStatusId);
            return View(friend);
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
            int selectedIndexCityDistrict = 1;

            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            int[] idFieldActivityUser = groupsUser.Select(g => g.FieldActivityId ?? 0).ToArray();
            int[] idOrganizationUser = groupsUser.Select(g => g.OrganizationId ?? 0).ToArray();

            ViewData["GroupUId"] = new SelectList(groupsUser, "IdGroup", "Name", friend.GroupUId);
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict, "IdCityDistrict", "Name", friend.CityDistrictId);
            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name", friend.ElectoralDistrictId);
            List<Fieldactivity> fieldactivitiesSelect = _context.Fieldactivity.Where(fac => idFieldActivityUser.Contains(fac.IdFieldActivity)).ToList();
            ViewData["FieldActivityId"] = new SelectList(fieldactivitiesSelect, "IdFieldActivity", "Name", friend.FieldActivityId);
            List<Organization> organizationSelect = _context.Organization.Where(org => idOrganizationUser.Contains(org.IdOrganization)).ToList();
            ViewData["OrganizationId"] = new SelectList(organizationSelect, "IdOrganization", "Name", friend.OrganizationId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", friend.MicroDistrictId);
            List<PollingStation> polingStations;
            if (friend.CityDistrictId != null)
            {
                 polingStations = _context.PollingStation.Where(p => p.CityDistrictId == friend.CityDistrictId).ToList().GroupBy(p => p.Name).Select(grp => grp.FirstOrDefault()).ToList();
            }
            else
            {
                 polingStations = _context.PollingStation.Where(p => p.CityDistrictId == selectedIndexCityDistrict).ToList().GroupBy(p => p.Name).Select(grp => grp.FirstOrDefault()).ToList();
            }
            //ViewData["PollingStationId"] = new SelectList(polingStations, "IdPollingStation", "Name", friend.PollingStationId);

            ///////
            int[] stationsId = polingStations.Select(p => p.StationId ?? 0).ToArray();
            List<Station> stations = _context.Station.Where(s => stationsId.Contains(s.IdStation)).ToList();
            ViewData["StationId"] = new SelectList(stations, "IdStation", "Name",friend.Station);
            //////

            List<Street> selectStreets = _context.Street.Where(s => s.CityId==friend.CityDistrictId).ToList();
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
            ViewData["FriendStatusId"] = new SelectList(_context.FriendStatus, "IdFriendStatus", "Name",friend.FriendStatusId);
            return View(friend);
        }

        // POST: Friends/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("IdFriend,FamilyName,Name,PatronymicName,DateBirth,Unpinning,CityId,CityDistrictId,ElectoralDistrictId,StreetId,MicroDistrictId,HouseId,Building,Apartment,Telephone,StationId,PollingStationId,Organization,FieldActivityId,PhoneNumberResponsible,DateRegistrationSite,VotingDate,Voter,Adress,TextQRcode,Qrcode,Email,Description,GroupUId,FriendStatusId,OrganizationId")] Friend friend)
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
                        friend.ByteQrcode = QRcodeServices.GenerateQRcodeFile(friend.FamilyName + " " + friend.Name + " " + friend.PatronymicName, friend.DateBirth.Value.Date.ToString("d"), NameServer + WayController + '?' + NameQRcodeParametrs + '=' + friend.TextQRcode, "png", WayPathQrCodes);
                        //friend.Qrcode = fileNameQRcode;

                        if (friend.Unpinning && friend.CityId!=1)
                        {
                            friend.CityDistrictId = null;
                            friend.StreetId = null;
                            friend.HouseId = null;
                            friend.Apartment = null;
                            ////???
                            //PollingStation pollingStationSearch = _context.PollingStation.Where(p => p.IdPollingStation == friend.PollingStationId).FirstOrDefault();
                            //friend.StationId = pollingStationSearch.StationId;
                            ////???
                        }
                        else
                        {
                            friend.Adress = null;
                        }

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
                    else ModelState.AddModelError("", "Данный избиратель уже был внесен в списки ранее!");
                }
                else ModelState.AddModelError("", "Не все поля были заполнены");
            }

            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            int[] idFieldActivityUser = groupsUser.Select(g => g.FieldActivityId ?? 0).ToArray();
            int[] idOrganizationUser = groupsUser.Select(g => g.OrganizationId ?? 0).ToArray();

            ViewData["GroupUId"] = new SelectList(groupsUser, "IdGroup", "Name", friend.GroupUId);
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict, "IdCityDistrict", "Name", friend.CityDistrictId);
            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name", friend.ElectoralDistrictId);
            List<Fieldactivity> fieldactivitiesSelect = _context.Fieldactivity.Where(fac => idFieldActivityUser.Contains(fac.IdFieldActivity)).ToList();
            ViewData["FieldActivityId"] = new SelectList(fieldactivitiesSelect, "IdFieldActivity", "Name", friend.FieldActivityId);
            List<Organization> organizationSelect = _context.Organization.Where(org => idOrganizationUser.Contains(org.IdOrganization)).ToList();
            ViewData["OrganizationId"] = new SelectList(organizationSelect, "IdOrganization", "Name", friend.OrganizationId);
            ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "Name", friend.HouseId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", friend.MicroDistrictId);
            ViewData["StationId"] = new SelectList(_context.Station, "IdStation", "Name", friend.StationId);
            ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "Name", friend.StreetId);
            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "UserName", friend.UserId);
            ViewData["FriendStatusId"] = new SelectList(_context.FriendStatus, "IdFriendStatus", "Name",friend.FriendStatusId);
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
                .Include(f => f.CityDistrict)
                .Include(f => f.ElectoralDistrict)
                .Include(f => f.FieldActivity)
                .Include(f => f.GroupU)
                .Include(f => f.House)
                .Include(f => f.MicroDistrict)
                .Include(f => f.Station)
                .Include(f => f.Street)
                .Include(f => f.User)
                .Include(f => f.FriendStatus)
                .Include(f => f.Organization_)
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
