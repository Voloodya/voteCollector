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
using voteCollector.Services;
using voteCollector.DTO;

namespace CollectVoters.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly VoterCollectorContext _context;
        private ServiceFriends _serviceFriends;
        private string NameServer;
        private string WayController;
        private string NameQRcodeParametrs;
        private ServiceUser _serviceUser;
        private ServiceGroup _serviceGroup;
        private string MinNameElectoralDistrict;
        private string WayPathQrCodes;

        public AdminController(VoterCollectorContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
            _serviceFriends = new ServiceFriends();
            _serviceGroup = new ServiceGroup(context);
            NameServer = "http://оренбургвсе.рф";
           // WayController = "/CollectVoters/api/QRcodeСheckAPI/checkqrcode";
            WayController = "/api/QRcodeСheckAPI/checkqrcode";
            NameQRcodeParametrs = "qrText";
            _serviceUser = new ServiceUser(context);
            MinNameElectoralDistrict = _context.ElectoralDistrict.Min(e => e.Name);
            WayPathQrCodes = "/wwwroot/qr_codes/";
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            List<Groupu> groupusLvl1 = _context.Groupu.Include(g => g.FieldActivity).Include(g => g.UserResponsible).Include(g => g.InverseGroupParents)
                .Where(g => g.Level == 1).Distinct().ToList();

            IQueryable<Friend> IQfriends = _context.Friend;
            List<Friend> friends = IQfriends.ToList();
            List<Report> reports = new List<Report>();
            
             
            foreach (Groupu grpLvl1 in groupusLvl1)
            {
                Report report = new Report
                {
                    Responseble = grpLvl1.UserResponsible != null ? grpLvl1.UserResponsible.FamilyName + " " + grpLvl1.UserResponsible.Name + " " + grpLvl1.UserResponsible.PatronymicName + " (" + grpLvl1.UserResponsible.Telephone + ")" : "",
                    IdOdject = grpLvl1.IdGroup,
                    NameObject = grpLvl1.FieldActivity.Name,
                    Level = grpLvl1.Level ?? 0,
                    NumberEmployees = grpLvl1.NumberEmployees ?? 0,
                    childGroup = false
                };
                int numberVoters = 0;
                int numberVoted = 0;
                int numberQRcodes = 0;

                List <Groupu> groupsChild = _serviceGroup.GetAllChildGroupsBFS(grpLvl1, grpLvl1, grpLvl1);
                
                foreach(Groupu groupu in groupsChild)
                {
                    if(groupu.IdGroup!=grpLvl1.IdGroup)  report.childGroup = true;
                    // _context.Entry(groupu).Collection(g => g.Friends).Load();
                    //numberVoters += groupu.Friends.Count();
                    //numberVoted += groupu.Friends.Where(f => f.Voter == true).Count();

                    numberVoters += friends.Where(f => f.GroupUId == groupu.IdGroup).Count();
                    numberVoted += friends.Where(f => f.GroupUId == groupu.IdGroup && f.Voter == true).Count();
                    numberQRcodes += friends.Where(f => f.GroupUId == groupu.IdGroup && f.TextQRcode != null && !f.TextQRcode.Equals("")).Count();
                }
                report.NumberVoters = numberVoters;
                report.NumberVoted = numberVoted;
                report.NumberQRcodesText = numberQRcodes;

                if (numberVoters != 0)
                {
                    report.PersentVotedByVoters = Math.Round((double) numberVoted / numberVoters *100, 2);
                }
                if (report.NumberEmployees != 0)
                {
                    report.PersentVotedByEmploees = Math.Round((double) numberVoted / report.NumberEmployees *100, 2);
                }
                if (report.NumberEmployees!=0) 
                {
                    report.PersentVotersByEmploees = Math.Round((double) numberVoters / report.NumberEmployees *100, 2);
                }
                reports.Add(report);
            }
            reports.Sort((r1, r2) => r1.NameObject.CompareTo(r2.NameObject));
            return View(reports);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("UserName", "Password", "ReturnUrl")] LoginModel loginViewModel)
        {
            return View();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet]
        public async Task<IActionResult> LkAdmin()
        {
            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

            if (groupsUser.Count > 0)
            {

                if (mainGroup != null && groupsUser.Contains(mainGroup))
                {
                    List<Friend> friends = new List<Friend>();
                    try
                    {
                        friends = _serviceFriends.GetAllFriendsLimit(100).ToList();
                    }
                    catch
                    {
                        friends = _serviceFriends.GetAllFriends().ToList();
                    }
                    return View(friends);
                }
                else
                {
                    List<Friend> friends = _serviceFriends.GetAllFriendsByGroupUsers(groupsUser).ToList();
                    return View(friends);
                }
            }
            return NoContent();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LkAdmin([Bind("UserName", "Password", "ReturnUrl")] LoginModel loginViewModel)
        {
            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

            if (groupsUser.Count > 0)
            {
                if (groupsUser.Contains(mainGroup))
                {
                    List<Friend> friends = new List<Friend>();
                    try
                    {
                        friends =  _serviceFriends.GetAllFriendsLimit(100).ToList();
                    }
                    catch
                    {
                        friends = _serviceFriends.GetAllFriends().ToList();
                    }
                    return View(friends);
                }
                else
                {
                    List<Friend> friends = _serviceFriends.GetAllFriendsByGroupUsers(groupsUser).ToList();
                    return View(friends);
                }
            }
            return NoContent();
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
                } catch(Exception ex) { }
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
            List<Groupu> filterGroupUser = _serviceUser.FilterGroups(groupsUser).ToList();
            int[] idFieldActivityUser = filterGroupUser.Select(g => g.FieldActivityId ?? 0).ToArray();
            int[] idOrganizationUser = filterGroupUser.Select(g => g.OrganizationId ?? 0).ToArray();


            ViewData["GroupUId"] = new SelectList(filterGroupUser, "IdGroup", "Name");
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", selectIndexCity);
            List<CityDistrict> cityDistricts = _context.CityDistrict.Where(cd => cd.CityId == selectIndexCity).ToList();
            ViewData["CityDistrictId"] = new SelectList(cityDistricts, "IdCityDistrict", "Name", selectedIndexCityDistrict);
            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name");
            List<Fieldactivity> fieldactivities = _context.Fieldactivity.Where(f => idFieldActivityUser.Contains(f.IdFieldActivity)).ToList();
            ViewData["FieldActivityId"] = new SelectList(fieldactivities, "IdFieldActivity", "Name", groupsUser[0].FieldActivityId);
            List<Organization> organization = _context.Organization.Where(org => idOrganizationUser.Contains(org.IdOrganization)).ToList();
            ViewData["OrganizationId"] = new SelectList(organization, "IdOrganization", "Name", groupsUser[0].OrganizationId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name");
            List<Street> selectStreets = _context.Street.Where(s => s.CityId == selectedIndexCityDistrict).ToList();
            ViewData["StreetId"] = new SelectList(selectStreets, "IdStreet", "Name");
            // ???
            IQueryable<House> selectHouse = _context.House.Where(h => h.StreetId == selectStreets[0].IdStreet);
            ViewData["HouseId"] = new SelectList(selectHouse, "IdHouse", "Name");

            //IQueryable<PollingStation> polingStations = _context.PollingStation.Where(p => p.CityId == 1).GroupBy(p => p.Name).Select(grp => grp.First());
            //IQueryable<PollingStation> filteredStations = _context.PollingStation.Where(p => p.CityId == 1);
            //var pollingStations = filteredStations.Where(p => p.IdPollingStation == filteredStations.Where(x => x.Name == p.Name).Min(y => y.IdPollingStation));
            var pollingStations = _context.PollingStation.Where(p => p.CityDistrictId == selectedIndexCityDistrict).ToList().GroupBy(p => p.Name).Select(grp => grp.FirstOrDefault());
            //ViewData["PollingStationId"] = new SelectList(pollingStations, "IdPollingStation", "Name");

            ///////
            int[] stationsId = pollingStations.Select(p => p.StationId ?? 0).ToArray();
            List<Station> stations = _context.Station.Where(s => stationsId.Contains(s.IdStation)).ToList();
            stations.Sort((s1, s2) => Convert.ToInt32(s1.Name) - Convert.ToInt32(s2.Name));
            ViewData["StationId"] = new SelectList(stations, "IdStation", "Name");
            //////

            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "FamilyName");
            ViewData["FriendStatusId"] = new SelectList(_context.FriendStatus, "IdFriendStatus", "Name");

            return View();
        }

        // POST: Friends/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFriend,FamilyName,Name,PatronymicName,DateBirth,Unpinning,CityId,CityDistrictId,ElectoralDistrictId,StreetId,MicroDistrictId,HouseId,Building,Apartment,Telephone,StationId,PollingStationId,Organization,FieldActivityId,PhoneNumberResponsible,DateRegistrationSite,VotingDate,Voter,Adress,TextQRcode,Qrcode,Description,UserId,GroupUId,FriendStatusId,OrganizationId")] Friend friend)
        {
            int selectedIndexCityDistrict = 1;

            if (ModelState.IsValid)
            {
                friend.FamilyName = friend.FamilyName != null ? friend.FamilyName.Trim() : null;
                friend.Name = friend.Name != null ? friend.Name.Trim() : null;
                friend.PatronymicName = friend.PatronymicName != null ? friend.PatronymicName.Trim() : null;
                friend.TextQRcode = friend.TextQRcode != null ? friend.TextQRcode.Trim() : null;

                List<Friend> searchFriend = _context.Friend.Include(f => f.GroupU).Where(frnd => frnd.Name.Trim().Equals(friend.Name) && frnd.FamilyName.Trim().Equals(friend.FamilyName) && frnd.PatronymicName.Trim().Equals(friend.PatronymicName) && frnd.DateBirth.Value.Date == friend.DateBirth.Value.Date && frnd.IdFriend != friend.IdFriend).ToList();

                if (friend.DateBirth.Value.Year < 1900 || friend.DateBirth.Value.Year > 2003)
                {
                    ModelState.AddModelError("", "Дата рождения меньше 1900 или больше 2003!");
                }
                else if (searchFriend.Count == 0)
                {
                    Friend friendQrText = null;
                    Friend friendNumberPhone = null;
                    if (friend.TextQRcode != null && !friend.TextQRcode.Trim().Equals(""))
                    {
                        friendQrText = _serviceFriends.FindUserByQRtext(friend.TextQRcode);
                    }
                    if (friend.Telephone != null && !friend.Telephone.Trim().Equals(""))
                    {
                        friendNumberPhone = _serviceFriends.FindUserByPhoneNumber(ServicePhoneNumber.LeaveOnlyNumbers(friend.Telephone));
                    }

                    if (friendQrText == null && friendNumberPhone == null)
                    {

                        friend.DateRegistrationSite = DateTime.Today;

                        User userSave = _context.User.Where(u => u.UserName.Equals(User.Identity.Name)).FirstOrDefault();
                        friend.UserId = userSave.IdUser;
                        friend.PhoneNumberResponsible = userSave.Telephone;
                        //friend.GroupUId = userSave.Groupsusers.First().GroupUId;
                        friend.ByteQrcode = QRcodeServices.GenerateQRcodeFile(friend.FamilyName + " " + friend.Name + " " + friend.PatronymicName, friend.DateBirth.Value.Date.ToString("d"), NameServer + WayController + '?' + NameQRcodeParametrs + '=' + friend.TextQRcode, "png", WayPathQrCodes);
                        //friend.Qrcode = fileNameQRcode;
                        friend.Telephone = ServicePhoneNumber.LeaveOnlyNumbers(friend.Telephone);

                        if (friend.CityId != 1)
                        {
                            friend.CityDistrictId = null;
                            friend.StreetId = null;
                            friend.HouseId = null;
                            friend.Apartment = null;

                            if (friend.Adress != null && friend.Adress.Trim().Length > 5)
                            {
                                if (friend.Unpinning)
                                {
                                    Station station;
                                    try
                                    {
                                        station = _context.Station.Find(friend.StationId);
                                    }
                                    catch
                                    {
                                        station = null;
                                    }
                                    if (station != null)
                                    {
                                        _context.Add(friend);
                                        await _context.SaveChangesAsync();
                                        return RedirectToAction(nameof(Index));
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("", "Не указан участок!");
                                    }
                                }
                                else if (!friend.Unpinning)
                                {
                                    friend.StationId = null;
                                    friend.ElectoralDistrictId = null;

                                    _context.Add(friend);
                                    await _context.SaveChangesAsync();
                                    return RedirectToAction(nameof(LkAdmin));
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Не указан участок!");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Не корректно заполнено поле с адресом!");
                            }
                        }
                        else
                        {
                            friend.Adress = null;

                            if (friend.HouseId != null && friend.StationId != null)
                            {
                                House house;
                                Station station;
                                try
                                {
                                    house = _context.House.Find(friend.HouseId);
                                    station = _context.Station.Find(friend.StationId);
                                }
                                catch
                                {
                                    house = null;
                                    station = null;
                                }
                                if (house != null && house.Name != null && !house.Name.Equals("") && !house.Name.Equals(" ") && station != null && station.Name != null && !station.Name.Equals(""))
                                {
                                    _context.Add(friend);
                                    await _context.SaveChangesAsync();
                                    return RedirectToAction(nameof(Index));
                                }
                                else ModelState.AddModelError("", "Не указан полный адрес или не выбран участок!");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Не указан полный адрес или не выбран участок!");
                            }
                        }

                    }
                    else ModelState.AddModelError("", "Учасник с данными телефоном или QR-кодом, уже был внесен в списки ранее!");

                }
                else ModelState.AddModelError("", "Участник с данными ФИО и датой рождения уже был внесен ранее в списки в " + searchFriend[0].GroupU.Name + "!");
            }

            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            List<Groupu> filterGroupUser = _serviceUser.FilterGroups(groupsUser).ToList();
            int[] idFieldActivityUser = filterGroupUser.Select(g => g.FieldActivityId ?? 0).ToArray();
            int[] idOrganizationUser = filterGroupUser.Select(g => g.OrganizationId ?? 0).ToArray();


            ViewData["GroupUId"] = new SelectList(filterGroupUser, "IdGroup", "Name", friend.GroupUId);
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
            List<CityDistrict> cityDistricts = _context.CityDistrict.Where(cd => cd.CityId == friend.CityId).ToList();
            ViewData["CityDistrictId"] = new SelectList(cityDistricts, "IdCityDistrict", "Name", friend.CityDistrictId);
            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name", friend.ElectoralDistrictId);
            List<Fieldactivity> fieldactivities = _context.Fieldactivity.Where(f => idFieldActivityUser.Contains(f.IdFieldActivity)).ToList();
            ViewData["FieldActivityId"] = new SelectList(fieldactivities, "IdFieldActivity", "Name", friend.FieldActivityId);
            List<Organization> organization = _context.Organization.Where(org => idOrganizationUser.Contains(org.IdOrganization)).ToList();
            ViewData["OrganizationId"] = new SelectList(organization, "IdOrganization", "Name", friend.OrganizationId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", friend.MicroDistrictId);


            List<Street> selectStreets = _context.Street.Where(s => s.CityId == friend.CityDistrictId).ToList();
            selectStreets.Sort((s1, s2) => s1.Name.CompareTo(s2.Name));
            ViewData["StreetId"] = new SelectList(selectStreets, "IdStreet", "Name", friend.StreetId);

            List<House> selectHouse = _context.House.Where(h => h.StreetId == friend.StreetId).ToList();
            selectHouse.Sort((h1, h2) => h1.Name.CompareTo(h2.Name));
            ViewData["HouseId"] = new SelectList(selectHouse, "IdHouse", "Name", friend.HouseId);

            if (friend.CityDistrictId == null || friend.Unpinning)
            {
                List<PollingStation> polingStations = _context.PollingStation.Where(p => p.CityDistrictId == selectedIndexCityDistrict).ToList().GroupBy(p => p.Name).Select(grp => grp.FirstOrDefault()).ToList();
                int[] stationsId = polingStations.Select(p => p.StationId ?? 0).ToArray();
                List<Station> stations = _context.Station.Where(s => stationsId.Contains(s.IdStation)).ToList();
                //stations.Sort((s1, s2) => Convert.ToInt32(s1.Name) - Convert.ToInt32(s2.Name));
                stations.Sort();
                stations.Insert(0, new Station { IdStation = 0, Name = "" });
                ViewData["StationId"] = new SelectList(stations, "IdStation", "Name", friend.StationId);
            }
            else
            {
                List<PollingStation> polingStations = _context.PollingStation.Where(p => p.CityDistrictId == friend.CityDistrictId).ToList().GroupBy(p => p.Name).Select(grp => grp.FirstOrDefault()).ToList();
                int[] stationsId = polingStations.Select(p => p.StationId ?? 0).ToArray();
                List<Station> stations = _context.Station.Where(s => stationsId.Contains(s.IdStation)).ToList();
                //stations.Sort((s1, s2) => Convert.ToInt32(s1.Name) - Convert.ToInt32(s2.Name));
                stations.Sort();
                stations.Insert(0, new Station { IdStation = 0, Name = "" });
                ViewData["StationId"] = new SelectList(stations, "IdStation", "Name", friend.StationId);
            }


            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "Name", friend.UserId);
            ViewData["FriendStatusId"] = new SelectList(_context.FriendStatus, "IdFriendStatus", "Name", friend.FriendStatusId);

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
            int selectedIndexCityDistrict = 1;

            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            List<Groupu> filterGroupUser = _serviceUser.FilterGroups(groupsUser).ToList();
            int[] idFieldActivityUser = filterGroupUser.Select(g => g.FieldActivityId ?? 0).ToArray();
            int[] idOrganizationUser = filterGroupUser.Select(g => g.OrganizationId ?? 0).ToArray();

            ViewData["GroupUId"] = new SelectList(filterGroupUser, "IdGroup", "Name", friend.GroupUId);
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
            List<CityDistrict> cityDistricts = _context.CityDistrict.Where(cd => cd.CityId == friend.CityId).ToList();
            ViewData["CityDistrictId"] = new SelectList(cityDistricts, "IdCityDistrict", "Name", friend.CityDistrictId);
            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name", friend.ElectoralDistrictId);
            List<Fieldactivity> fieldactivities = _context.Fieldactivity.Where(f => idFieldActivityUser.Contains(f.IdFieldActivity)).ToList();
            ViewData["FieldActivityId"] = new SelectList(fieldactivities, "IdFieldActivity", "Name", friend.FieldActivityId);
            List<Organization> organization = _context.Organization.Where(org => idOrganizationUser.Contains(org.IdOrganization)).ToList();
            ViewData["OrganizationId"] = new SelectList(organization, "IdOrganization", "Name", friend.OrganizationId);
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
            ViewData["StationId"] = new SelectList(stations, "IdStation", "Name", friend.Station);
            //////

            List<Street> selectStreets = _context.Street.Where(s => s.CityId == friend.CityDistrictId).ToList();
            ViewData["StreetId"] = new SelectList(selectStreets, "IdStreet", "Name", friend.StreetId);
            List<House> selectHouse = _context.House.Where(h => h.StreetId == friend.StreetId).ToList();
            ViewData["HouseId"] = new SelectList(selectHouse, "IdHouse", "Name", friend.HouseId);
            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "Name", friend.UserId);
            ViewData["FriendStatusId"] = new SelectList(_context.FriendStatus, "IdFriendStatus", "Name", friend.FriendStatusId);

            return View(friend);
        }

        // POST: Friends/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("IdFriend,FamilyName,Name,PatronymicName,DateBirth,Unpinning,CityId,CityDistrictId,ElectoralDistrictId,StreetId,MicroDistrictId,HouseId,Building,Apartment,Telephone,StationId,PollingStationId,Organization,FieldActivityId,PhoneNumberResponsible,DateRegistrationSite,VotingDate,Voter,Adress,TextQRcode,Qrcode,Description,GroupUId,FriendStatusId,OrganizationId")] Friend friend)
        {
            if (id != friend.IdFriend)
            {
                return NotFound();
            }
            int selectedIndexCityDistrict = 1;

            if (ModelState.IsValid)
            {
                DateTime dateEmpty = new DateTime();
                if (!friend.Name.Equals("") && !friend.FamilyName.Equals("") && friend.DateBirth != dateEmpty)
                {
                    friend.FamilyName = friend.FamilyName != null ? friend.FamilyName.Trim() : null;
                    friend.Name = friend.Name != null ? friend.Name.Trim() : null;
                    friend.PatronymicName = friend.PatronymicName != null ? friend.PatronymicName.Trim() : null;
                    friend.TextQRcode = friend.TextQRcode != null ? friend.TextQRcode.Trim() : null;

                    List<Friend> searchFriend = _context.Friend.Include(f => f.GroupU).Where(frnd => frnd.Name.Trim().Equals(friend.Name) && frnd.FamilyName.Trim().Equals(friend.FamilyName) && frnd.PatronymicName.Trim().Equals(friend.PatronymicName) && frnd.DateBirth.Value.Date == friend.DateBirth.Value.Date && frnd.IdFriend != friend.IdFriend).ToList();

                    if (friend.DateBirth.Value.Year < 1900 || friend.DateBirth.Value.Year > 2003)
                    {
                        ModelState.AddModelError("", "Дата рождения меньше 1900 или больше 2003!");
                    }
                    else if (searchFriend.Count == 0)
                    {
                        Friend friendQrText = null;
                        Friend friendNumberPhone = null;
                        if (friend.TextQRcode != null && !friend.TextQRcode.Trim().Equals(""))
                        {
                            friendQrText = _serviceFriends.FindUserByQRtext(friend.TextQRcode);
                        }
                        if (friend.Telephone != null && !friend.Telephone.Trim().Equals(""))
                        {
                            friendNumberPhone = _serviceFriends.FindUserByPhoneNumber(ServicePhoneNumber.LeaveOnlyNumbers(friend.Telephone));
                        }

                        if ((friendQrText == null || friendQrText.IdFriend == friend.IdFriend) && (friendNumberPhone == null || friendNumberPhone.IdFriend == friend.IdFriend)) { 

                            User userSave = _context.User.Where(u => u.UserName.Equals(User.Identity.Name)).FirstOrDefault();
                        //friend.UserId = userSave.IdUser;
                        //friend.GroupUId = userSave.Groupsusers.First().GroupUId;
                        friend.ByteQrcode = QRcodeServices.GenerateQRcodeFile(friend.FamilyName + " " + friend.Name + " " + friend.PatronymicName, friend.DateBirth.Value.Date.ToString("d"), NameServer + WayController + '?' + NameQRcodeParametrs + '=' + friend.TextQRcode, "png", WayPathQrCodes);
                        //friend.Qrcode = fileNameQRcode;

                        if (friend.CityId != 1)
                        {
                            friend.CityDistrictId = null;
                            friend.StreetId = null;
                            friend.HouseId = null;
                            friend.Apartment = null;
                            if (friend.Adress != null && friend.Adress.Trim().Length > 5)
                            {
                                if (friend.Unpinning)
                                {
                                    Station station;
                                    try
                                    {
                                        station = _context.Station.Find(friend.StationId);
                                    }
                                    catch
                                    {
                                        station = null;
                                    }
                                    if (station != null)
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
                                    else
                                    {
                                        ModelState.AddModelError("", "Не указан участок!");
                                    }
                                }
                                else if (!friend.Unpinning)
                                {
                                    friend.StationId = null;
                                    friend.ElectoralDistrictId = null;
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
                                    return RedirectToAction(nameof(LkAdmin));
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Не указан участок!");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", "Не корректно заполнено поле с адресом!");
                            }

                        }
                        else
                        {
                            friend.Adress = null;

                            if (friend.HouseId != null && friend.StationId != null)
                            {
                                House house;
                                Station station;
                                try
                                {
                                    house = _context.House.Find(friend.HouseId);
                                    station = _context.Station.Find(friend.StationId);
                                }
                                catch
                                {
                                    house = null;
                                    station = null;
                                }
                                if (house != null && house.Name != null && !house.Name.Equals("") && !house.Name.Equals(" ") && station != null && station.Name != null && !station.Name.Equals(""))
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
                                else ModelState.AddModelError("", "Не указан полный адрес или не выбран участок!");

                            }
                            else
                            {
                                ModelState.AddModelError("", "Не указан полный адрес или не выбран участок!");
                            }
                        }
                    }
                        else ModelState.AddModelError("", "Участник с данными телефоном или QR-кодом, уже был внесен в списки ранее!");
                    }
                    else ModelState.AddModelError("", "Участник с данными ФИО и датой рождения уже был внесен ранее в списки в " + searchFriend[0].GroupU.Name + "!");
                }
                else ModelState.AddModelError("", "Не все поля были заполнены");
            }

            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            List<Groupu> filterGroupUser = _serviceUser.FilterGroups(groupsUser).ToList();
            int[] idFieldActivityUser = filterGroupUser.Select(g => g.FieldActivityId ?? 0).ToArray();
            int[] idOrganizationUser = filterGroupUser.Select(g => g.OrganizationId ?? 0).ToArray();

            ViewData["GroupUId"] = new SelectList(filterGroupUser, "IdGroup", "Name", friend.GroupUId);
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
            List<CityDistrict> cityDistricts = _context.CityDistrict.Where(cd => cd.CityId == friend.CityId).ToList();
            ViewData["CityDistrictId"] = new SelectList(cityDistricts, "IdCityDistrict", "Name", friend.CityDistrictId);
            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name", friend.ElectoralDistrictId);
            List<Fieldactivity> fieldactivities = _context.Fieldactivity.Where(f => idFieldActivityUser.Contains(f.IdFieldActivity)).ToList();
            ViewData["FieldActivityId"] = new SelectList(fieldactivities, "IdFieldActivity", "Name", friend.FieldActivityId);
            List<Organization> organization = _context.Organization.Where(org => idOrganizationUser.Contains(org.IdOrganization)).ToList();
            ViewData["OrganizationId"] = new SelectList(organization, "IdOrganization", "Name", friend.OrganizationId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", friend.MicroDistrictId);

            List<Street> selectStreets = _context.Street.Where(s => s.CityId == friend.CityDistrictId).ToList();
            selectStreets.Sort((s1, s2) => s1.Name.CompareTo(s2.Name));
            ViewData["StreetId"] = new SelectList(selectStreets, "IdStreet", "Name", friend.StreetId);
            List<House> selectHouse = _context.House.Where(h => h.StreetId == friend.StreetId).ToList();
            selectHouse.Sort((h1, h2) => h1.Name.CompareTo(h2.Name));
            ViewData["HouseId"] = new SelectList(selectHouse, "IdHouse", "Name", friend.HouseId);

            if (friend.CityDistrictId == null || friend.Unpinning)
            {
                List<PollingStation> polingStations = _context.PollingStation.Where(p => p.CityDistrictId == selectedIndexCityDistrict).ToList().GroupBy(p => p.Name).Select(grp => grp.FirstOrDefault()).ToList();
                int[] stationsId = polingStations.Select(p => p.StationId ?? 0).ToArray();
                List<Station> stations = _context.Station.Where(s => stationsId.Contains(s.IdStation)).ToList();
                //stations.Sort((s1, s2) => Convert.ToInt32(s1.Name) - Convert.ToInt32(s2.Name));
                stations.Sort();
                stations.Insert(0, new Station { IdStation = 0, Name = "" });
                ViewData["StationId"] = new SelectList(stations, "IdStation", "Name", friend.StationId);
            }
            else
            {
                List<PollingStation> polingStations = _context.PollingStation.Where(p => p.CityDistrictId == friend.CityDistrictId).ToList().GroupBy(p => p.Name).Select(grp => grp.FirstOrDefault()).ToList();
                int[] stationsId = polingStations.Select(p => p.StationId ?? 0).ToArray();
                List<Station> stations = _context.Station.Where(s => stationsId.Contains(s.IdStation)).ToList();
                //stations.Sort((s1, s2) => Convert.ToInt32(s1.Name) - Convert.ToInt32(s2.Name));
                stations.Sort();
                stations.Insert(0, new Station { IdStation = 0, Name = "" });
                ViewData["StationId"] = new SelectList(stations, "IdStation", "Name", friend.StationId);
            }

            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "UserName", friend.UserId);
            ViewData["FriendStatusId"] = new SelectList(_context.FriendStatus, "IdFriendStatus", "Name", friend.FriendStatusId);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchFriendsByElectoralDistrict([FromBody] ElectoralDistrictDTO electoralDistrictDTO)
        {

            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

            List<Friend> friends;

            if (mainGroup != null && groupsUser.Contains(mainGroup))
            {
                friends = await _serviceFriends.SearchFriendsByElectoralDistrict(electoralDistrictDTO).ToListAsync();
            }
            else
            {
                friends = await _serviceFriends.SearchFriendsByElectoralDistrictAndGroupsUsers(electoralDistrictDTO, groupsUser).ToListAsync();
            }

            return PartialView(friends);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchFriendsByFieldActivity([FromBody] FieldActivityDTO fieldActivityDTO)
        {
            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

            List<Friend> friends=null;

            if (fieldActivityDTO.LimitUpload != null)
            {
                if (mainGroup != null && groupsUser.Contains(mainGroup))
                {
                    friends = await _serviceFriends.SearchFriendsByFieldActiviteLimit(fieldActivityDTO, fieldActivityDTO.LimitUpload.Value).ToListAsync();
                }
                else
                {
                    friends = await _serviceFriends.SearchFriendsByFieldActiviteAndGroupsUsersLimit(fieldActivityDTO, groupsUser, fieldActivityDTO.LimitUpload.Value).ToListAsync();
                }
            }
            else
            {
                if (mainGroup != null && groupsUser.Contains(mainGroup))
                {
                    friends = await _serviceFriends.SearchFriendsByFieldActivite(fieldActivityDTO).ToListAsync();
                }
                else
                {
                    friends = await _serviceFriends.SearchFriendsByFieldActiviteAndGroupsUsers(fieldActivityDTO, groupsUser).ToListAsync();
                }
            }

            return PartialView(friends);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchFriendsByAllFieldActivity([FromBody] FieldActivityDTO fieldActivityDTO)
        {
            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

            List<Friend> friends = null;

            if(fieldActivityDTO.LimitUpload != null)
            {
                if (mainGroup != null && groupsUser.Contains(mainGroup))
                {
                    friends = await _serviceFriends.GetAllFriendsLimit(fieldActivityDTO.LimitUpload.Value).ToListAsync();
                }
                else
                {
                    friends = await _serviceFriends.GetAllFriendsByGroupUsersLimit(groupsUser, fieldActivityDTO.LimitUpload.Value).ToListAsync();
                }
            }
            else
            {
                if (mainGroup != null && groupsUser.Contains(mainGroup))
                {
                    friends = await _serviceFriends.GetAllFriends().ToListAsync();
                }
                else
                {
                    friends = await _serviceFriends.GetAllFriendsByGroupUsers(groupsUser).ToListAsync();
                }
            }

            return PartialView(friends);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchFriendsByOrganization([FromBody] OrganizationDTO organizationDTO)
        {

            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

            List<Friend> friends = null;

            if (organizationDTO.LimitUpload != null)
            {
                if (mainGroup != null && groupsUser.Contains(mainGroup))
                {
                    friends = await _serviceFriends.SearchFriendsByOrganizationLimit(organizationDTO, organizationDTO.LimitUpload.Value).ToListAsync();
                }
                else
                {
                    friends = await _serviceFriends.SearchFriendsByOrganizationAndGroupsUsersLimit(organizationDTO, groupsUser, organizationDTO.LimitUpload.Value).ToListAsync();
                }
            }
            else
            {
                if (mainGroup != null && groupsUser.Contains(mainGroup))
                {
                    friends = await _serviceFriends.SearchFriendsByOrganization(organizationDTO).ToListAsync();
                }
                else
                {
                    friends = await _serviceFriends.SearchFriendsByOrganizationAndGroupsUsers(organizationDTO, groupsUser).ToListAsync();
                }
            }

            return PartialView(friends);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchFriendsByGroup([FromBody] GroupDTO groupDTO)
        {
            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

            List<Friend> friends = null;

            if (groupDTO.LimitUpload != null)
            {
                if (mainGroup != null && groupsUser.Contains(mainGroup))
                {
                    friends = await _serviceFriends.SearchFriendsByGroupLimit(groupDTO, groupDTO.LimitUpload.Value).ToListAsync();
                }
                else
                {
                    friends = await _serviceFriends.SearchFriendsByGroupAndGroupsUsersLimit(groupDTO, groupsUser, groupDTO.LimitUpload.Value).ToListAsync();
                }
            }
            else
            {
                if (mainGroup != null && groupsUser.Contains(mainGroup))
                {
                    friends = await _serviceFriends.SearchFriendsByGroup(groupDTO).ToListAsync();
                }
                else
                {
                    friends = await _serviceFriends.SearchFriendsByGroupAndGroupsUsers(groupDTO, groupsUser).ToListAsync();
                }
            }
            return PartialView(friends);
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
