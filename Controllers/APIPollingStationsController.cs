using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using voteCollector.Data;
using voteCollector.DTO;
using voteCollector.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace voteCollector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class APIPollingStationsController : ControllerBase
    {

        private readonly VoterCollectorContext _context;

        public APIPollingStationsController(VoterCollectorContext context)
        {
            _context = context;
        }


        // POST api/<APIPollingStationsController>
        [HttpPost("SearchPolingStationsByElectoralDistrict")]
        public IActionResult SearchPolingStationsByElectoralDistrict([FromBody]ElectoralDistrictDTO electoralDistrictDTO)
        {
            List<District> districts = _context.District.Where(d => d.ElectoralDistrictId == electoralDistrictDTO.IdElectoralDistrict).ToList();
            List<int> selectStationsId = districts.Select(d => d.StationId ?? 0).ToList();

            List<PollingStation> pollingStations = _context.PollingStation.Include(p => p.CityDistrict).Include(p => p.Street).Include(p => p.House)
                .Where(p => selectStationsId.Contains(p.StationId ?? 0)).ToList();

            List<PollingStationDTO> pollingStationDTOs = pollingStations.Select(
            p => new PollingStationDTO { IdPollingStation = p.IdPollingStation, Name = p.Name, CityName = p.CityDistrict.Name, StreetName = p.Street.Name, HouseName = p.House.Name }).ToList();

            return Ok(pollingStationDTOs);
        }

    }
}
