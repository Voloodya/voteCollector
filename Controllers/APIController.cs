using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using voteCollector.Data;
using voteCollector.DTO;
using voteCollector.Models;
using voteCollector.Services;

namespace voteCollector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles = "admin, user")]
    public class APIController : ControllerBase
    {
        private readonly VoterCollectorContext _context;
        private string NameServer;
        private string WayController;
        private string NameQRcodeParametrs;
        private string WayPathQrCodes;
        private readonly ILogger<APIController> _logger;
        public APIController(VoterCollectorContext context, ILogger<APIController> logger)
        {
            _context = context;
            NameServer = "http://оренбургвсе.рф";
            // WayController = "/CollectVoters/api/QRcodeСheckAPI/checkqrcode";
            WayController = "/api/QRcodeСheckAPI/checkqrcode";
            NameQRcodeParametrs = "qrText";
            WayPathQrCodes = "/wwwroot/qr_codes/";
            _logger = logger;
        }

        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        [HttpGet("GetSitiesDistricts/{idcity}")]
        public IActionResult GetSitiesDistricts(int idcity)
        {
            List<CityDistrict> Cities = _context.CityDistrict.Where(cd => cd.CityId == idcity).ToList();

            if (Cities.Any())
            {
                List<CityDTO> citiesDTO = Cities.Select(s => new CityDTO { IdCity = s.IdCityDistrict, Name = s.Name }).ToList();
                return Ok(citiesDTO);
            }
            return NoContent();
        }

        [HttpGet("getSities")]
        public IActionResult GetSities()
        {
            List<City> Cities = _context.City.ToList();

            if (Cities.Any())
            {
                List<CityDTO> citiesDTO = Cities.Select(s => new CityDTO { IdCity = s.IdCity, Name = s.Name }).ToList();
                return Ok(citiesDTO);
            }
            return NoContent();
        }


        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        [HttpGet("getElectoralDistrict")]
        public IActionResult GetElectoralDistrict()
        {
            List<ElectoralDistrict> electoralDistricts = _context.ElectoralDistrict.ToList();

            if (electoralDistricts.Any())
            {
                List<ElectoralDistrictDTO> electoralDistrictsDTO = electoralDistricts.Select(s => new ElectoralDistrictDTO { IdElectoralDistrict = s.IdElectoralDistrict, Name = s.Name }).ToList();
                electoralDistrictsDTO.Add(new ElectoralDistrictDTO { IdElectoralDistrict = 0, Name = " Все" });
                return Ok(electoralDistrictsDTO);
            }
            return NoContent();
        }

        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        [HttpGet("getfieldactivite")]
        public IActionResult GetFieldActivite()
        {
            List<Fieldactivity> fieldactivities = _context.Fieldactivity.ToList();

            if (fieldactivities.Any())
            {
                List<FieldActivityDTO> fieldActivityDTOs = fieldactivities.Select(f => new FieldActivityDTO { IdFieldActivity = f.IdFieldActivity, Name = f.Name }).ToList();
                fieldActivityDTOs.Add(new FieldActivityDTO { IdFieldActivity = 0, Name = " Все" });
                return Ok(fieldActivityDTOs);
            }
            return NoContent();
        }


        [HttpGet("getorganization/{idFielfActivity}")]
        public IActionResult GetOrganizationActivity(int idFielfActivity)
        {
            List<Groupu> groups = _context.Groupu.Where(g => g.FieldActivityId == idFielfActivity && g.Level == 2).ToList();
            List<int> idOrganizations = groups.Select(g => g.OrganizationId ?? 0).ToList().Distinct().ToList(); //??
            List<OrganizationDTO> organizationDTOs = new List<OrganizationDTO>();

            if (idOrganizations.Count>0)
            {
                List<Organization> organizations = _context.Organization.Where(org => idOrganizations.Contains(org.IdOrganization)).ToList();
                organizationDTOs = organizations.Select(org => new OrganizationDTO { IdOrganization = org.IdOrganization, Name = org.Name }).ToList();
                organizationDTOs.Add(new OrganizationDTO { IdOrganization = 0, Name = " Все" });
                return Ok(organizationDTOs);
            }
            return Ok(organizationDTOs);
        }

        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        [HttpGet("getorganizationall")]
        public IActionResult GetOrganizationAll()
        {

            List<Organization> organizations = _context.Groupu.Where(g => g.Level == 2).Select(g => g.Organization).Distinct().ToList();

            List<OrganizationDTO> organizationDTOs = new List<OrganizationDTO>();
            if (organizations.Count > 0)
            {
                organizationDTOs = organizations.Select(org => new OrganizationDTO { IdOrganization = org.IdOrganization, Name = org.Name }).ToList();
                organizationDTOs.Add(new OrganizationDTO { IdOrganization = 0, Name = " Все" });
                return Ok(organizationDTOs);
            }
            return Ok(organizationDTOs);
        }

        [HttpGet("getgroupsbyorganization/{idorganization}")]
        public IActionResult GetGroupsByOrganization(int idorganization)
        {
            List<Groupu> groups = _context.Groupu.Where(g => g.OrganizationId == idorganization).ToList();
            int maxLevel = groups.Max(g => g.Level ?? 0);
            List<Groupu> groupsSearch = groups.Where(g => g.Level>=maxLevel).ToList();
            List<GroupDTO> groupDTOs = new List<GroupDTO>();

            if (groupsSearch.Count > 0)
            {
                groupDTOs = groupsSearch.Select(g => new GroupDTO { IdGroup = g.IdGroup, Name = g.Name }).ToList();
                groupDTOs.Add( new GroupDTO { IdGroup = 0, Name = " Все" });
                return Ok(groupDTOs);
            }
            return Ok(groupDTOs);
        }

        [HttpGet("GetGroupsMaxLvlByOrganization/{idorganization}")]
        public IActionResult GetGroupsMaxLvlByOrganization(int idorganization)
        {
            List<Groupu> groups = _context.Groupu.Where(g => g.OrganizationId == idorganization).ToList();
            int maxLevel = groups.Max(g => g.Level ?? 0);
            List<Groupu> groupsSearch = groups.Where(g => g.Level >= maxLevel).ToList();
            List<GroupDTO> groupDTOs = new List<GroupDTO>();

            if (groupsSearch.Count > 0)
            {
                groupDTOs = groupsSearch.Select(g => new GroupDTO { IdGroup = g.IdGroup, Name = g.Name }).ToList();
                return Ok(groupDTOs);
            }
            return Ok(groupDTOs);
        }

        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        [HttpGet("getgroupsall")]
        public IActionResult GetGroupsAll()
        {
            List<Groupu> groups = _context.Groupu.Where(g => g.Level>=3).ToList();
            List<GroupDTO> groupDTOs = new List<GroupDTO>();

            if (groups.Count > 0)
            {
                groupDTOs = groups.Select(g => new GroupDTO { IdGroup = g.IdGroup, Name = g.Name }).ToList();
                groupDTOs.Add(new GroupDTO { IdGroup = 0, Name = " Все" });
                return Ok(groupDTOs);
            }
            return Ok(groupDTOs);
        }

        [HttpPost("searchStreets")]
        public IActionResult SearchStreets(CityDTO citySelected)
        {
            List<Street> streets =  _context.Street.Where(s => s.CityId == citySelected.IdCity).ToList<Street>();
            List<StreetDTO> streetsDTO = new List<StreetDTO>();
            if (streets.Any())
            {
                streetsDTO = streets.Select(s => new StreetDTO { IdStreet = s.IdStreet, Name = s.Name }).ToList();
                return Ok(streetsDTO);                
            }
            return Ok(streetsDTO);
        }

        [HttpPost("searchHouse")]
        public IActionResult SearchHouse(StreetDTO streetSelected)
        {
            List<House> houses = _context.House.Where(h => h.StreetId == streetSelected.IdStreet).ToList<House>();
            List<HouseDTO> housesDTO = new List<HouseDTO>();
            if (houses.Any())
            {
                housesDTO = houses.Select(h => new HouseDTO { IdHouse = h.IdHouse, Name = h.Name }).ToList();
                return Ok(housesDTO);
            }
            return Ok(housesDTO);
        }

        [HttpPost("searchPollingStations/city")]
        public IActionResult SearchPollingStationsCity(CityDTO cityDTO)
        {
            List<PollingStation> pollingStations = _context.PollingStation.Where(ps => ps.CityDistrictId == cityDTO.IdCity).ToList().GroupBy(p => p.Name).Select(grp => grp.First()).ToList();

            if (pollingStations.Any())
            {
                List<PollingStationDTO> pollingStationsDTO = pollingStations.Select(p => new PollingStationDTO { IdPollingStation = p.IdPollingStation, Name = p.Name }).ToList();
                pollingStationsDTO.Insert(0, new PollingStationDTO { IdPollingStation = 0, Name = "" });
                return Ok(pollingStationsDTO);
            }
            return NoContent();
        }

        [HttpPost("searchStations/city")]
        public IActionResult SearchStationsCity(CityDTO cityDTO)
        {
            List<PollingStation> pollingStations;
            if (cityDTO.Name == null || cityDTO.Name.Equals("") || cityDTO.Name.Equals(" "))
            {
                List<Station> stations = _context.Station.ToList();
                List<StationDTO> stationDTOs = stations.Select(s => new StationDTO { IdStation = s.IdStation, Name = s.Name }).ToList();
                stationDTOs.Insert(0, new StationDTO { IdStation = 0, Name = "" });
                return Ok(stationDTOs);
            }
            else
            {
                pollingStations = _context.PollingStation.Where(ps => ps.CityDistrictId == cityDTO.IdCity).ToList().GroupBy(p => p.Name).Select(grp => grp.First()).ToList();
            }

            if (pollingStations != null && pollingStations.Any())
            {
                int[] stationsId = pollingStations.Select(p => p.StationId ?? 0).ToArray();

                if (stationsId.Any())
                {
                    List<Station> stations = _context.Station.Where(s => stationsId.Contains(s.IdStation)).ToList();
                    List<StationDTO> stationDTOs = stations.Select(s => new StationDTO { IdStation = s.IdStation, Name = s.Name }).ToList();
                    stationDTOs.Insert(0, new StationDTO { IdStation = 0, Name = "" });
                    return Ok(stationDTOs);
                }
                else
                {
                    return NoContent();
                }
            }
            else { return NoContent();}
        }

        [HttpPost("searchPollingStations/street")]
        public IActionResult SearchPollingStationsStreet(StreetDTO street)
        {
            List<PollingStation> pollingStations = _context.PollingStation.Where(ps => ps.StreetId == street.IdStreet).ToList().GroupBy(p => p.Name).Select(grp => grp.First()).ToList();

            if (pollingStations.Any())
            {
                List<PollingStationDTO> pollingStationsDTO = pollingStations.Select(p => new PollingStationDTO { IdPollingStation = p.IdPollingStation, Name = p.Name }).ToList();
                return Ok(pollingStationsDTO);
            }
            return NoContent();
        }

        [HttpPost("searchStations/street")]
        public IActionResult SearchStationsStreet(StreetDTO street)
        {
            List<PollingStation> pollingStations = _context.PollingStation.Where(ps => ps.StreetId == street.IdStreet).ToList().GroupBy(p => p.Name).Select(grp => grp.First()).ToList();

            if (pollingStations.Any())
            {
                int[] stationsId = pollingStations.Select(p => p.StationId ?? 0).ToArray();

                if (stationsId.Any())
                {
                    List<Station> stations = _context.Station.Where(s => stationsId.Contains(s.IdStation)).ToList();
                    List<StationDTO> stationDTOs = stations.Select(s => new StationDTO { IdStation = s.IdStation, Name = s.Name }).ToList();
                    stationDTOs.Insert(0, new StationDTO { IdStation = 0, Name = "" });
                    return Ok(stationDTOs);
                }
                else
                {
                    return NoContent();
                }
            }
            else { return NoContent(); }
        }


        [HttpPost("searchPollingStations/house")]
        public IActionResult SearchPollingStationsHouse(HouseDTO house)
        {
            var pollingStations = _context.PollingStation.Where(ps => ps.HouseId == house.IdHouse);

            if(pollingStations.Any())
            {
                List<PollingStationDTO> pollingStationsDTO = pollingStations.Select(p => new PollingStationDTO { IdPollingStation = p.IdPollingStation, Name = p.Name }).ToList();
                return Ok(pollingStationsDTO);
            }
            return NoContent();
        }

        [HttpPost("searchStations/house")]
        public IActionResult SearchStationsHouse(HouseDTO house)
        {
            List<PollingStation> pollingStations = _context.PollingStation.Where(ps => ps.HouseId == house.IdHouse).ToList().GroupBy(p => p.Name).Select(grp => grp.First()).ToList();

            if (pollingStations.Any())
            {
                int[] stationsId = pollingStations.Select(p => p.StationId ?? 0).ToArray();

                if (stationsId.Any())
                {
                    List<Station> stations = _context.Station.Where(s => stationsId.Contains(s.IdStation)).ToList();
                    List<StationDTO> stationDTOs = stations.Select(s => new StationDTO { IdStation = s.IdStation, Name = s.Name }).ToList();
                    return Ok(stationDTOs);
                }
                else
                {
                    return NoContent();
                }
            }
            else { return NoContent(); }
        }

        [HttpPost("searchStations/streetAndhouse")]
        public IActionResult SearchStationsByStreetAndHouse(StreetHouseDTO streetHouseDTO)
        {
            List<PollingStation> pollingStations = _context.PollingStation.Where(ps => ps.StreetId == streetHouseDTO.IdStreet && ps.HouseId == streetHouseDTO.IdHouse).ToList().GroupBy(p => p.Name).Select(grp => grp.First()).ToList();

            if (pollingStations.Any())
            {
                int[] stationsId = pollingStations.Select(p => p.StationId ?? 0).ToArray();

                if (stationsId.Any())
                {
                    List<Station> stations = _context.Station.Where(s => stationsId.Contains(s.IdStation)).ToList();
                    List<StationDTO> stationDTOs = stations.Select(s => new StationDTO { IdStation = s.IdStation, Name = s.Name }).ToList();
                    return Ok(stationDTOs);
                }
                else
                {
                    return NoContent();
                }
            }
            else { return NoContent(); }
        }


        [HttpPost("searchElectoraldistrict/station")]
        public IActionResult SearchElectoralDistrictByStation(StationDTO stationDTO)
        {

            if (stationDTO != null)
            {
                District district = _context.District.Where(d => d.StationId == stationDTO.IdStation).FirstOrDefault();
                List<ElectoralDistrict> electoralDistrict = _context.ElectoralDistrict.Where(ed => ed.IdElectoralDistrict == district.ElectoralDistrictId).ToList();

                if (electoralDistrict.Any())
                {

                    List<ElectoralDistrictDTO> electoralDistrictDTOs = electoralDistrict.Select(ed => new ElectoralDistrictDTO { IdElectoralDistrict = ed.IdElectoralDistrict, Name = ed.Name }).ToList();

                    return Ok(electoralDistrictDTOs);
                }
                else
                {
                    return NoContent();
                }
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost("searchElectoraldistrict/stations")]
        public IActionResult SearchElectoralDistrictByStations(StationDTO [] stationDTOs)
        {

            if (stationDTOs != null && stationDTOs.Length>0)
            {
                List<int> idStationsSearch = stationDTOs.Select(s => s.IdStation).ToList();
                List<District> districts = _context.District.Where(d => idStationsSearch.Contains(d.StationId ?? 0)).ToList();
                List<int> idElectralDistrictsSearh = districts.Select(ed => ed.ElectoralDistrictId ?? 0).ToList();
                List<ElectoralDistrict> electoralDistrict = _context.ElectoralDistrict.Where(ed => idElectralDistrictsSearh.Contains(ed.IdElectoralDistrict)).ToList();

                if (electoralDistrict.Any())
                {

                    List<ElectoralDistrictDTO> electoralDistrictDTOs = electoralDistrict.Select(ed => new ElectoralDistrictDTO { IdElectoralDistrict = ed.IdElectoralDistrict, Name = ed.Name }).ToList();
                    electoralDistrictDTOs.Insert(0, new ElectoralDistrictDTO { IdElectoralDistrict = 0, Name = "" });

                    return Ok(electoralDistrictDTOs);
                }
                else
                {
                    return NoContent();
                }
            }
            else
            {
                return NoContent();
            }
        }

        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 600)]
        [HttpGet("getAllElectoraldistrict")]
        public IActionResult GetAllElectoraldistrict()
        {
                List<ElectoralDistrict> electoralDistrict = _context.ElectoralDistrict.ToList();

                if (electoralDistrict.Any())
                {

                    List<ElectoralDistrictDTO> electoralDistrictDTOs = electoralDistrict.Select(ed => new ElectoralDistrictDTO { IdElectoralDistrict = ed.IdElectoralDistrict, Name = ed.Name }).ToList();
                    electoralDistrictDTOs.Insert(0, new ElectoralDistrictDTO { IdElectoralDistrict = 0, Name = "" });

                return Ok(electoralDistrictDTOs);
                }
                else
                {
                    return NoContent();
                }
        }

        [HttpPost("CountNumberVoters")]
        public IActionResult CountNumberVoters(StationDTO[] stationDTOs)
        {

            return Ok();
        }


        [HttpGet("RegenerateQRCodes")]
        public IActionResult RegenerateQRCodes()
        {
            List<string> errors = new List<string>();
            List<Friend> friends = _context.Friend.Where(f => f.TextQRcode != null && !f.TextQRcode.Trim().Equals("")).ToList();

            foreach (Friend frnd in friends)
            {
                //frnd.ByteQrcode = QRcodeServices.GenerateQRcodeFile(frnd.FamilyName + " " + frnd.Name + " " + frnd.PatronymicName, frnd.DateBirth.Value.Date.ToString("d"), NameServer + WayController + '?' + NameQRcodeParametrs + '=' + frnd.TextQRcode, "png", WayPathQrCodes);
                frnd.ByteQrcode = QRcodeServices.GenerateQRcodeFile(frnd.FamilyName + " " + frnd.Name + " " + frnd.PatronymicName, frnd.DateBirth.Value.Date.ToString("d"),frnd.TextQRcode, "png", WayPathQrCodes);

                try
                {
                    _context.Update(frnd);
                     _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    errors.Add("Ошибка: "+frnd.IdFriend.ToString());
                    continue;
                }
            }

            return Ok(errors);
        }

        [HttpGet("CheackStationsAndDistricts")]
        public async Task<IActionResult> CheackStationsAndDistricts()
        {
            int countSeccefful = 0;
            int countError = 0;

            List<FriendDTOShot> friendDTOs = new List<FriendDTOShot>();

            List<Friend> friends = _context.Friend.Where(f => f.CityId == 1 && f.Unpinning==false).ToList();

            foreach(Friend friend in friends)
            {
                int idStation = 0;
                int idElectoralDistrict = 0;

                    PollingStation pollingStation = null;
                    if (friend.HouseId != null)
                    {
                        try
                        {
                            pollingStation = _context.PollingStation.FirstOrDefault(p => p.HouseId == friend.HouseId);
                        }
                        catch
                        {
                            countError++;
                            continue;
                        }
                    }
                    else
                    {
                        countError++;
                        continue;
                    }

                    if (pollingStation != null)
                    {
                        idStation = pollingStation.StationId ?? 0;
                        try
                        {
                            idElectoralDistrict = _context.District.FirstOrDefault(d => d.StationId == idStation).ElectoralDistrictId ?? 0;
                        }
                        catch
                        {
                            countError++;
                            continue;
                        }
                    }
                else
                {
                    countError++;
                    continue;
                }
                    //friendDTOs.Add(new FriendDTOShot { Name = friend.Name, FamilyName = friend.FamilyName, PatronymicName = friend.PatronymicName,
                    //    FieldActivityName = friend.FieldActivity.Name, Organization = friend.Organization_.Name, Group = friend.GroupU.Name,
                    //     City = friend.City.Name, Street = friend.Street.Name, House = friend.House.Name});

                if(idStation != 0 && idElectoralDistrict != 0)
                {
                    friend.StationId = idStation;
                    friend.ElectoralDistrictId = idElectoralDistrict;
                    try
                    {
                        _context.Update(friend);                        
                        countSeccefful++;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        countError++;
                        continue;
                        //friendDTOs.Add(new FriendDTOShot
                        //{
                        //    Name = friend.Name,
                        //    FamilyName = friend.FamilyName,
                        //    PatronymicName = friend.PatronymicName,
                        //    FieldActivityName = friend.FieldActivity.Name,
                        //    Organization = friend.Organization_.Name,
                        //    Group = friend.GroupU.Name,
                        //    City = friend.City.Name,
                        //    Street = friend.Street.Name,
                        //    House = friend.House.Name
                        //});
                    }
                }
            }
            try
            {
                _context.SaveChanges();
            }
            catch
            {
                return Ok("Изменения не сохранены!");

            }
            return Ok("Выгружено записей: " + friends.Count +" Успешно обновленных записей: " + countSeccefful.ToString() + " Необновленных: " + countError.ToString());
            //return Ok(friendDTOs);

        }

        [HttpGet("PostRequestQRcodes")]
        public IActionResult PostRequestQRcodes()
        {
            return Ok();
        }

    }
}
