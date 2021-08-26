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

            List<Groupu> groupusLvl1 = _context.Groupu.Include(g => g.FieldActivity).Include(g => g.UserResponsible).Include(g => g.Friends)
                .Where(g => g.Level == 1).Distinct().ToList();

            List<Report> reports = new List<Report>();
            
             
            foreach (Groupu grpLvl1 in groupusLvl1)
            {
                Report report = new Report
                {
                    Responseble = grpLvl1.UserResponsible != null ? grpLvl1.UserResponsible.FamilyName + " " + grpLvl1.UserResponsible.Name + " " + grpLvl1.UserResponsible.PatronymicName + " (" + grpLvl1.UserResponsible.Telephone + ")" : "",
                    IdOdject = grpLvl1.FieldActivityId ?? 0,
                    NameObject = grpLvl1.FieldActivity.Name,
                    Level = grpLvl1.Level ?? 0,
                    NumberEmployees = grpLvl1.NumberEmployees ?? 0
                };
                int numberVoters = 0;
                int numberVoted = 0;

                List <Groupu> groupsChild = _serviceGroup.GetAllChildGroupsBFS(grpLvl1, grpLvl1, grpLvl1);

                foreach(Groupu groupu in groupsChild)
                {
                    numberVoters += groupu.Friends.Count();
                    numberVoted += groupu.Friends.Where(f => f.Voter == true).Count();
                }
                report.NumberVoters = numberVoters;
                report.NumberVoted = numberVoted;
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

            //List<Report> report = await _context.Groupu.Include(g => g.FieldActivity).Include(g => g.UserResponsible).Include(g => g.Friends)
            //    .Where(g => g.Level == 1).Distinct().Select(g => new Report
            //{
            //    Responseble = g.UserResponsible.FamilyName +" "+ g.UserResponsible.Name + " " + g.UserResponsible.PatronymicName,
            //    IdOdject = g.FieldActivityId ?? 0,
            //    NameObject = g.FieldActivity.Name,
            //    Level = g.Level ?? 0,
            //    NumberEmployees = g.NumberEmployees ?? 0,
            //    NumberVoters = g.Friends.Count,
            //    NumberVoted = g.Friends.Select(f => f.Voter==true).Count(),
            //}).Distinct().ToListAsync();


            return View(reports);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("UserName", "Password", "ReturnUrl")] LoginModel loginViewModel)
        {
            List<Report> report = await _context.Groupu.Include(g => g.FieldActivity).Include(g => g.UserResponsible).Where(g => g.Level == 1).Distinct().Select(g => new Report
            {
                Responseble = g.UserResponsible.FamilyName + " " + g.UserResponsible.Name + " " + g.UserResponsible.PatronymicName,
                IdOdject = g.FieldActivityId ?? 0,
                NameObject = g.FieldActivity.Name,
                Level = g.Level ?? 0,
                NumberEmployees = g.NumberEmployees ?? 0,
            }).Distinct().ToListAsync();

            return View(report);
        }

        [HttpGet]
        public async Task<IActionResult> ReportOrganization(int id)
        {
            List<Groupu> groupus = _context.Groupu.Include(g => g.Organization).Include(g => g.UserResponsible).Where(g => g.Level == 2 && g.FieldActivityId == id).ToList()
                .GroupBy(g => g.OrganizationId).Select(g => g.First()).ToList();

            List<Groupu> groupus1 = groupus;

            List<Report> reports = groupus.Select(g => new Report
            {
                Responseble = g.UserResponsible.FamilyName+" "+ g.UserResponsible.Name + " "+ g.UserResponsible.PatronymicName,
                IdOdject = g.OrganizationId ?? 0,
                NameObject = g.Organization.Name,
                Level = g.Level ?? 0,
                NumberEmployees = g.NumberEmployees ?? 0,
                IdParent = id,
                NameParent = g.FieldActivity.Name
            }).ToList();

            return View(reports);
        }

        [HttpGet]
        public async Task<IActionResult> ReportGroup(int id)
        {
            List<Report> report = await _context.Groupu.Include(g => g.Organization).Include(g => g.UserResponsible).Where(g => g.Level == 3 && g.OrganizationId == id)
                .Distinct().Select(g => new Report
                {
                    Responseble = g.UserResponsible.FamilyName + " " + g.UserResponsible.Name + " " + g.UserResponsible.PatronymicName,
                    IdOdject = g.IdGroup,
                    NameObject = g.Name,
                    Level = g.Level ?? 0,
                    NumberEmployees = g.NumberEmployees ?? 0,
                    IdParent = id,
                    NameParent = g.Organization.Name
                }).ToListAsync();

            return View(report);
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
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict, "IdCityDistrict", "Name", selectedIndexCityDistrict);
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
                List<Friend> searchFriend = _context.Friend.Where(frnd => frnd.Name.Equals(friend.Name) && frnd.FamilyName.Equals(friend.FamilyName) && frnd.PatronymicName.Equals(friend.PatronymicName) && frnd.DateBirth.Value.Date == friend.DateBirth.Value.Date).ToList();

                if (searchFriend.Count == 0)
                {
                    friend.DateRegistrationSite = DateTime.Today;

                    User userSave = _context.User.Where(u => u.UserName.Equals(User.Identity.Name)).FirstOrDefault();
                    friend.UserId = userSave.IdUser;
                    friend.PhoneNumberResponsible = userSave.Telephone;
                    //friend.GroupUId = userSave.Groupsusers.First().GroupUId;
                    friend.ByteQrcode = QRcodeServices.GenerateQRcodeFile(friend.FamilyName + " " + friend.Name + " " + friend.PatronymicName, friend.DateBirth.Value.Date.ToString("d"), NameServer + WayController + '?' + NameQRcodeParametrs + '=' + friend.TextQRcode, "png", WayPathQrCodes);
                    //friend.Qrcode = fileNameQRcode;
                    friend.Telephone = ServicePhoneNumber.LeaveOnlyNumbers(friend.Telephone);

                    if (friend.Unpinning)
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
                    return RedirectToAction(nameof(LkAdmin));
                }
                else ModelState.AddModelError("", "Данный избиратель уже был внесен в списки ранее!");
            }

            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            List<Groupu> filterGroupUser = _serviceUser.FilterGroups(groupsUser).ToList();
            int[] idFieldActivityUser = filterGroupUser.Select(g => g.FieldActivityId ?? 0).ToArray();
            int[] idOrganizationUser = filterGroupUser.Select(g => g.OrganizationId ?? 0).ToArray();


            ViewData["GroupUId"] = new SelectList(filterGroupUser, "IdGroup", "Name", friend.GroupUId);
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name", friend.ElectoralDistrictId);
            List<Fieldactivity> fieldactivities = _context.Fieldactivity.Where(f => idFieldActivityUser.Contains(f.IdFieldActivity)).ToList();
            ViewData["FieldActivityId"] = new SelectList(fieldactivities, "IdFieldActivity", "Name", friend.FieldActivityId);
            List<Organization> organization = _context.Organization.Where(org => idOrganizationUser.Contains(org.IdOrganization)).ToList();
            ViewData["OrganizationId"] = new SelectList(organization, "IdOrganization", "Name", friend.OrganizationId);
            ViewData["StationId"] = new SelectList(_context.Station, "IdStation", "Name", friend.StationId);
            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "Name", friend.UserId);
            ViewData["FriendStatusId"] = new SelectList(_context.FriendStatus, "IdFriendStatus", "Name", friend.FriendStatusId);

            if (!friend.Unpinning && friend.CityId != 1)
            {
                ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict, "IdCityDistrict", "Name", friend.CityDistrictId);
                ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", friend.MicroDistrictId);
                List<Street> selectStreets = _context.Street.Where(s => s.CityId == friend.CityDistrictId).ToList();
                ViewData["StreetId"] = new SelectList(selectStreets, "IdStreet", "Name", friend.StreetId);
                // ???
                List<House> selectHouse = _context.House.Where(h => h.StreetId == selectStreets[0].IdStreet).ToList();
                ViewData["HouseId"] = new SelectList(selectHouse, "IdHouse", "Name", friend.HouseId);
            }
            else
            {
                ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict, "IdCityDistrict", "Name", selectedIndexCityDistrict);
                ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", friend.MicroDistrictId);
                List<Street> selectStreets = _context.Street.Where(s => s.CityId == selectedIndexCityDistrict).ToList();
                ViewData["StreetId"] = new SelectList(selectStreets, "IdStreet", "Name", friend.StreetId);
                // ???
                List<House> selectHouse = _context.House.Where(h => h.StreetId == selectStreets[0].IdStreet).ToList();
                ViewData["HouseId"] = new SelectList(selectHouse, "IdHouse", "Name", friend.HouseId);
            }

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
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict, "IdCityDistrict", "Name", friend.CityDistrictId);
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

            if (ModelState.IsValid)
            {
                DateTime dateEmpty = new DateTime();
                if (!friend.Name.Equals("") && !friend.FamilyName.Equals("") && friend.DateBirth != dateEmpty)
                {
                    List<Friend> searchFriend = _context.Friend.Where(frnd => frnd.Name.Equals(friend.Name) && frnd.FamilyName.Equals(friend.FamilyName) && frnd.PatronymicName.Equals(friend.PatronymicName) && frnd.DateBirth.Value.Date == friend.DateBirth.Value.Date && frnd.IdFriend != friend.IdFriend).ToList();

                    if (searchFriend.Count == 0)
                    {
                        User userSave = _context.User.Where(u => u.UserName.Equals(User.Identity.Name)).FirstOrDefault();
                        //friend.UserId = userSave.IdUser;
                        //friend.GroupUId = userSave.Groupsusers.First().GroupUId;
                        friend.ByteQrcode = QRcodeServices.GenerateQRcodeFile(friend.FamilyName + " " + friend.Name + " " + friend.PatronymicName, friend.DateBirth.Value.Date.ToString("d"), NameServer + WayController + '?' + NameQRcodeParametrs + '=' + friend.TextQRcode, "png", WayPathQrCodes);
                        //friend.Qrcode = fileNameQRcode;

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
                    else ModelState.AddModelError("", "Данный избиратель уже был внесен в списки ранее!");
                }
                else ModelState.AddModelError("", "Не все обязательные поля были заполнены!");
            }

            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            List<Groupu> filterGroupUser = _serviceUser.FilterGroups(groupsUser).ToList();
            int[] idFieldActivityUser = filterGroupUser.Select(g => g.FieldActivityId ?? 0).ToArray();
            int[] idOrganizationUser = filterGroupUser.Select(g => g.OrganizationId ?? 0).ToArray();

            ViewData["GroupUId"] = new SelectList(filterGroupUser, "IdGroup", "Name", friend.GroupUId);
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict, "IdCityDistrict", "Name", friend.CityDistrictId);
            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name", friend.ElectoralDistrictId);
            List<Fieldactivity> fieldactivities = _context.Fieldactivity.Where(f => idFieldActivityUser.Contains(f.IdFieldActivity)).ToList();
            ViewData["FieldActivityId"] = new SelectList(fieldactivities, "IdFieldActivity", "Name", friend.FieldActivityId);
            List<Organization> organization = _context.Organization.Where(org => idOrganizationUser.Contains(org.IdOrganization)).ToList();
            ViewData["OrganizationId"] = new SelectList(organization, "IdOrganization", "Name", friend.OrganizationId);
            ViewData["HouseId"] = new SelectList(_context.House, "IdHouse", "Name", friend.HouseId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", friend.MicroDistrictId);
            ViewData["StationId"] = new SelectList(_context.Station, "IdStation", "Name", friend.StationId);
            ViewData["StreetId"] = new SelectList(_context.Street, "IdStreet", "Name", friend.StreetId);
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
