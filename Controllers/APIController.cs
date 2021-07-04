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


        [HttpPost("searchStreets")]
        [ValidateAntiForgeryToken]
        public IActionResult SearchStreets(CityDTO citySelected)
        {
            List<Street> streets =  _context.Street.Where(s => s.CityId == citySelected.CityId).ToList<Street>();
            
            if (streets.Any())
            {
                List<StreetDTO> streetsDTO = streets.Select(s => new StreetDTO { IdStreet = s.IdStreet, Name = s.Name }).ToList();
                return Ok(streetsDTO);                
            }
            return NoContent();
        }

        [HttpPost("searchHouse")]
        [ValidateAntiForgeryToken]
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

        [HttpPost("searchPollingStations/street")]
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
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

    }
}
