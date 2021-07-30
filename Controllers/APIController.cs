using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
            List<City> Cities = _context.City.ToList();

            if (Cities.Any())
            {
                List<CityDTO> citiesDTO = Cities.Select(s => new CityDTO { IdCity = s.IdCity, Name = s.Name }).ToList();
                return Ok(citiesDTO);
            }
            return NoContent();
        }

        [HttpGet("getElectoralDistrict")]
        public IActionResult GetgetElectoralDistrict()
        {
            List<ElectoralDistrict> electoralDistricts = _context.ElectoralDistrict.ToList();

            if (electoralDistricts.Any())
            {
                List<ElectoralDistrictDTO> electoralDistrictsDTO = electoralDistricts.Select(s => new ElectoralDistrictDTO { IdElectoralDistrict = s.IdElectoralDistrict, Name = s.Name }).ToList();
                return Ok(electoralDistrictsDTO);
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
        public IActionResult SearchPollingStationsCity(StreetDTO street)
        {
            List<PollingStation> pollingStations = _context.PollingStation.Where(ps => ps.StreetId == street.IdStreet).ToList().GroupBy(p => p.Name).Select(grp => grp.First()).ToList();

            if (pollingStations.Any())
            {
                List<PollingStationDTO> pollingStationsDTO = pollingStations.Select(p => new PollingStationDTO { IdPollingStation = p.IdPollingStation, Name = p.Name }).ToList();
                return Ok(pollingStationsDTO);
            }
            return NoContent();
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

        [HttpPost("searchElectoraldistrict/pollingstation")]
        public IActionResult SearchElectoralDistrictByPollingStation(PollingStationDTO pollingStationDTO)
        {
            PollingStation pollingStation = _context.PollingStation.Where(ps => ps.Name.Equals(pollingStationDTO.Name)).FirstOrDefault();

            if (pollingStation!=null)
            {
                Station station = _context.Station.Where(s => s.IdStation == pollingStation.StationId).FirstOrDefault();

                if (station != null)
                {
                    District district = _context.District.Where(d => d.StationId == station.IdStation).FirstOrDefault();
                    List<ElectoralDistrict> electoralDistrict = _context.ElectoralDistrict.Where(ed => ed.IdElectoralDistrict == district.ElectoralDistrictId).ToList();

                    List<ElectoralDistrictDTO> electoralDistrictDTOs = electoralDistrict.Select(ed => new ElectoralDistrictDTO { IdElectoralDistrict=ed.IdElectoralDistrict, Name=ed.Name }).ToList();

                    return Ok(electoralDistrictDTOs);
                }
                else
                {
                    return NoContent();
                }
            }
            return NoContent();
        }

    }
}
