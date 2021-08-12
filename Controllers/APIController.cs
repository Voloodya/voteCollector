using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using voteCollector.Data;
using voteCollector.DTO;
using voteCollector.Models;

namespace voteCollector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class APIController : ControllerBase
    {
        private readonly VoterCollectorContext _context;

        public APIController(VoterCollectorContext context)
        {
            _context = context;
        }

        [HttpGet("getSities")]
        public IActionResult GetSities()
        {
            List<CityDistrict> Cities = _context.CityDistrict.ToList();

            if (Cities.Any())
            {
                List<CityDTO> citiesDTO = Cities.Select(s => new CityDTO { IdCity = s.IdCityDistrict, Name = s.Name }).ToList();
                return Ok(citiesDTO);
            }
            return NoContent();
        }

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
            List<Groupu> groups = _context.Groupu.Include(g => g.Organization).Where(g => g.FieldActivityId == idFielfActivity).ToList();
            List<int> idOrganizations = groups.Select(g => g.Organization.IdOrganization).ToList().Distinct().ToList();


            if (idOrganizations.Count>0)
            {
                List<Organization> organizations = _context.Organization.Where(org => idOrganizations.Contains(org.IdOrganization)).ToList();
                List<OrganizationDTO> organizationDTOs = organizations.Select(org => new OrganizationDTO { IdOrganization = org.IdOrganization, Name = org.Name }).ToList();
                organizationDTOs.Add(new OrganizationDTO { IdOrganization = 0, Name = " Все" });
                return Ok(organizationDTOs);
            }
            return NoContent();
        }

        [HttpGet("getorganizationall")]
        public IActionResult GetOrganizationAll()
        {

            List<Organization> organizations = _context.Organization.ToList();

            if (organizations.Count > 0)
            {
                List<OrganizationDTO> organizationDTOs = organizations.Select(org => new OrganizationDTO { IdOrganization = org.IdOrganization, Name = org.Name }).ToList();
                organizationDTOs.Add(new OrganizationDTO { IdOrganization = 0, Name = " Все" });
                return Ok(organizationDTOs);
            }
            return NoContent();
        }

        [HttpGet("getgroupsbyorganization/{idorganization}")]
        public IActionResult GetGroupsByOrganization(int idorganization)
        {
            List<Groupu> groups = _context.Groupu.Where(g => g.OrganizationId == idorganization).ToList();

            if (groups.Count > 0)
            {
                List<GroupDTO> groupDTOs = groups.Select(g => new GroupDTO { IdGroup = g.IdGroup, Name = g.Name }).ToList();
                groupDTOs.Add( new GroupDTO { IdGroup = 0, Name = " Все" });
                return Ok(groupDTOs);
            }
            return NoContent();
        }

        [HttpGet("getgroupsall")]
        public IActionResult GetGroupsAll()
        {
            List<Groupu> groups = _context.Groupu.ToList();

            if (groups.Count > 0)
            {
                List<GroupDTO> groupDTOs = groups.Select(g => new GroupDTO { IdGroup = g.IdGroup, Name = g.Name }).ToList();
                groupDTOs.Add(new GroupDTO { IdGroup = 0, Name = " Все" });
                return Ok(groupDTOs);
            }
            return NoContent();
        }


        [HttpPost("searchStreets")]
        public IActionResult SearchStreets(CityDTO citySelected)
        {
            List<Street> streets =  _context.Street.Where(s => s.CityId == citySelected.IdCity).ToList<Street>();
            
            if (streets.Any())
            {
                List<StreetDTO> streetsDTO = streets.Select(s => new StreetDTO { IdStreet = s.IdStreet, Name = s.Name }).ToList();
                return Ok(streetsDTO);                
            }
            return NoContent();
        }

        [HttpPost("searchHouse")]
        public IActionResult SearchHouse(StreetDTO streetSelected)
        {
            List<House> houses = _context.House.Where(h => h.StreetId == streetSelected.IdStreet).ToList<House>();

            if (houses.Any())
            {
                List<HouseDTO> housesDTO = houses.Select(h => new HouseDTO { IdHouse = h.IdHouse, Name = h.Name }).ToList();
                return Ok(housesDTO);
            }
            return NoContent();
        }

        [HttpPost("searchPollingStations/city")]
        public IActionResult SearchPollingStationsCity(CityDTO cityDTO)
        {
            List<PollingStation> pollingStations = _context.PollingStation.Where(ps => ps.CityId == cityDTO.IdCity).ToList().GroupBy(p => p.Name).Select(grp => grp.First()).ToList();

            if (pollingStations.Any())
            {
                List<PollingStationDTO> pollingStationsDTO = pollingStations.Select(p => new PollingStationDTO { IdPollingStation = p.IdPollingStation, Name = p.Name }).ToList();
                return Ok(pollingStationsDTO);
            }
            return NoContent();
        }

        [HttpPost("searchStations/city")]
        public IActionResult SearchStationsCity(CityDTO cityDTO)
        {
            List<PollingStation> pollingStations = _context.PollingStation.Where(ps => ps.CityId == cityDTO.IdCity).ToList().GroupBy(p => p.Name).Select(grp => grp.First()).ToList();

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

    }
}
