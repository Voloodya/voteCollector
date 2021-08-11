﻿using voteCollector.Data;
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
        private string MinNameElectoralDistrict;

        public AdminController(VoterCollectorContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
            _serviceFriends = new ServiceFriends();
            NameServer = "http://195.226.209.40";
            WayController = "/CollectVoters/api/QRcodeСheckAPI/checkqrcode";
            NameQRcodeParametrs = "qrText";
            _serviceUser = new ServiceUser(context);
            MinNameElectoralDistrict = _context.ElectoralDistrict.Min(e => e.Name);
        }

        // GET: Friends
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

            if (mainGroup!=null && groupsUser.Contains(mainGroup))
            {
                List<Friend> friends = await _serviceFriends.SearchFriendsByNameElectoralDistrict(MinNameElectoralDistrict).ToListAsync();
                //var voterCollectorContext = _context.Friend.Include(f => f.City).Include(f => f.ElectoralDistrict).Include(f => f.FieldActivity).Include(f => f.GroupU).Include(f => f.House).Include(f => f.MicroDistrict).Include(f => f.PollingStation).Include(f => f.Street).Include(f => f.User);
                //List<Friend> friends = await voterCollectorContext.ToListAsync();
                return View(friends);
            }
            else
            {
                List<Friend> friends = await _serviceFriends.SearchFriendsByNameElectoralDistrictAndGroups(MinNameElectoralDistrict, groupsUser).ToListAsync();

                return View(friends);
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
                List<Friend> friends = await _serviceFriends.SearchFriendsByNameElectoralDistrict(MinNameElectoralDistrict).ToListAsync();
                return View(friends);
            }
            else
            {
                List<Friend> friends = await _serviceFriends.SearchFriendsByNameElectoralDistrictAndGroups(MinNameElectoralDistrict, groupsUser).ToListAsync();
                return View(friends);
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

            if (friend.Qrcode != null && !friend.Qrcode.Equals(""))
            {
                try
                {
                    friend.QRcodeBytes = QRcodeServices.BitmapToBytes(QRcodeServices.ReadingQRcodeFromFile(friend.Qrcode));
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

            ViewData["GroupUId"] = new SelectList(_serviceUser.FilterGroups(groupsUser), "IdGroup", "Name");
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", selectIndexCity);
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict, "IdCityDistrict", "Name", selectedIndexCityDistrict);
            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name");
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name", groupsUser[0].FieldActivityId);
            ViewData["OrganizationId"] = new SelectList(_context.Organization, "IdOrganization", "Name", groupsUser[0].OrganizationId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name");
            List<Street> selectStreets = _context.Street.Where(s => s.CityId == selectedIndexCityDistrict).ToList();
            ViewData["StreetId"] = new SelectList(selectStreets, "IdStreet", "Name");
            // ???
            IQueryable<House> selectHouse = _context.House.Where(h => h.StreetId == selectStreets[0].IdStreet);
            ViewData["HouseId"] = new SelectList(selectHouse, "IdHouse", "Name");

            //IQueryable<PollingStation> polingStations = _context.PollingStation.Where(p => p.CityId == 1).GroupBy(p => p.Name).Select(grp => grp.First());
            //IQueryable<PollingStation> filteredStations = _context.PollingStation.Where(p => p.CityId == 1);
            //var pollingStations = filteredStations.Where(p => p.IdPollingStation == filteredStations.Where(x => x.Name == p.Name).Min(y => y.IdPollingStation));
            var pollingStations = _context.PollingStation.Where(p => p.CityId == selectedIndexCityDistrict).ToList().GroupBy(p => p.Name).Select(grp => grp.FirstOrDefault());
            //ViewData["PollingStationId"] = new SelectList(pollingStations, "IdPollingStation", "Name");

            ///////
            int[] stationsId = pollingStations.Select(p => p.StationId ?? 0).ToArray();
            List<Station> stations = _context.Station.Where(s => stationsId.Contains(s.IdStation)).ToList();
            ViewData["StationId"] = new SelectList(stations, "IdStation", "Name");
            //////

            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "FamilyName");
            ViewData["FriendStatusId"] = new SelectList(_context.FriendStatus, "IdFriendStatus", "Name");

            return View();
        }

        // POST: Friends/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFriend,FamilyName,Name,PatronymicName,DateBirth,Unpinning,CityId,ElectoralDistrictId,StreetId,MicroDistrictId,HouseId,Building,Apartment,Telephone,StationId,PollingStationId,Organization,FieldActivityId,PhoneNumberResponsible,DateRegistrationSite,VotingDate,Voter,Adress,TextQRcode,Qrcode,Description,UserId,GroupUId,FriendStatusId,OrganizationId")] Friend friend)
        {
            int selectedIndexCityDistrict = 1;

            if (ModelState.IsValid)
            {
                List<Friend> searchFriend = _context.Friend.Where(frnd => frnd.Name.Equals(friend.Name) && frnd.FamilyName.Equals(friend.FamilyName) && frnd.PatronymicName.Equals(friend.PatronymicName) && frnd.DateBirth.Value.Date == friend.DateBirth.Value.Date).ToList();

                if (searchFriend.Count == 0)
                {
                    if (!friend.Unpinning)
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
                    }
                    else
                    {
                        friend.CityDistrictId = null;
                        friend.StreetId = null;
                        friend.HouseId = null;
                        friend.Apartment = null;
                    }

                    _context.Add(friend);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else ModelState.AddModelError("", "Данный избиратель уже был внесен в списки ранее!");
            }

            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);

            ViewData["GroupUId"] = new SelectList(_serviceUser.FilterGroups(groupsUser), "IdGroup", "Name", friend.GroupUId);
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name", friend.ElectoralDistrictId);
            List<Fieldactivity> fieldactivitiesSelect = _context.Fieldactivity.ToList();
            ViewData["FieldActivityId"] = new SelectList(fieldactivitiesSelect, "IdFieldActivity", "Name", friend.FieldActivityId);
            ViewData["OrganizationId"] = new SelectList(_context.Organization, "IdOrganization", "Name", friend.OrganizationId);
            ViewData["StationId"] = new SelectList(_context.Station, "IdStation", "Name", friend.StationId);
            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "Name", friend.UserId);
            ViewData["FriendStatusId"] = new SelectList(_context.FriendStatus, "IdFriendStatus", "Name", friend.FriendStatusId);

            if (!friend.Unpinning)
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

            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);

            ViewData["GroupUId"] = new SelectList(_serviceUser.FilterGroups(groupsUser), "IdGroup", "Name", friend.GroupUId);
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict, "IdCityDistrict", "Name", friend.CityDistrictId);
            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name", friend.ElectoralDistrictId);
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name", friend.FieldActivityId);
            ViewData["OrganizationId"] = new SelectList(_context.Organization, "IdOrganization", "Name", friend.OrganizationId);
            ViewData["MicroDistrictId"] = new SelectList(_context.Microdistrict, "IdMicroDistrict", "Name", friend.MicroDistrictId);
            var polingStations = _context.PollingStation.Where(p => p.CityId == friend.CityDistrictId).ToList().GroupBy(p => p.Name).Select(grp => grp.FirstOrDefault());
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
        public async Task<IActionResult> Edit(long id, [Bind("IdFriend,FamilyName,Name,PatronymicName,DateBirth,Unpinning,CityId,ElectoralDistrictId,StreetId,MicroDistrictId,HouseId,Building,Apartment,Telephone,StationId,PollingStationId,Organization,FieldActivityId,PhoneNumberResponsible,DateRegistrationSite,VotingDate,Voter,Adress,TextQRcode,Qrcode,Description,UserId,GroupUId,FriendStatusId,OrganizationId")] Friend friend)
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
                        if (!friend.Unpinning)
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
                        }
                        else
                        {
                            friend.CityDistrictId = null;
                            friend.StreetId = null;
                            friend.HouseId = null;
                            friend.Apartment = null;
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
                else ModelState.AddModelError("", "Не все обязательные поля были заполнены!");
            }

            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);

            ViewData["GroupUId"] = new SelectList(_serviceUser.FilterGroups(groupsUser), "IdGroup", "Name", friend.GroupUId);
            ViewData["CityId"] = new SelectList(_context.City, "IdCity", "Name", friend.CityId);
            ViewData["CityDistrictId"] = new SelectList(_context.CityDistrict, "IdCityDistrict", "Name", friend.CityDistrictId);
            ViewData["ElectoralDistrictId"] = new SelectList(_context.ElectoralDistrict, "IdElectoralDistrict", "Name", friend.ElectoralDistrictId);
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name", friend.FieldActivityId);
            ViewData["OrganizationId"] = new SelectList(_context.Organization, "IdOrganization", "Name", friend.OrganizationId);
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
                friends = await _serviceFriends.SearchFriendsByElectoralDistrictAndGroups(electoralDistrictDTO, groupsUser).ToListAsync();
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

            if (mainGroup != null && groupsUser.Contains(mainGroup))
            {
                friends = await _serviceFriends.SearchFriendsByFieldActivite(fieldActivityDTO).ToListAsync();
            }
            else
            {
                friends = await _serviceFriends.SearchFriendsByFieldActiviteAndGroups(fieldActivityDTO, groupsUser).ToListAsync();
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
