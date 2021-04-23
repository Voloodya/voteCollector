using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using voteCollector.Data;
using voteCollector.DTO;
using voteCollector.Models;

namespace voteCollector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreetsAPIController : ControllerBase
    {
        private readonly VoterCollectorContext _context;

        public StreetsAPIController(VoterCollectorContext context)
        {
            _context = context;
        }

        // GET: api/StreetsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Street>>> GetStreet()
        {
            return await _context.Street.ToListAsync();
        }

        // GET: api/StreetsAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Street>> GetStreet(int id)
        {
            var street = await _context.Street.FindAsync(id);

            if (street == null)
            {
                return NotFound();
            }

            return street;
        }

        [HttpPost("search")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<IEnumerable<Street>>> GetStreets(CityDTO citySelected)
        {
            var cites = await _context.Street.Where(s => s.CityId == citySelected.CityId).ToListAsync();
            return cites;
        }


        [HttpPost]
        public async Task<ActionResult<Street>> PostStreet(Street street)
        {
            _context.Street.Add(street);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStreet", new { id = street.IdStreet }, street);
        }


    }
}
