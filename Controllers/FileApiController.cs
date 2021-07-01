using CollectVoters.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using voteCollector.Data;
using voteCollector.Models;

namespace voteCollector.Controllers
{
    [Route("api/FileApi/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FileApiController : ControllerBase
    {
        private readonly VoterCollectorContext _context;

        public FileApiController(VoterCollectorContext context)
        {
            _context = context;
        }

        [HttpPost("uploadDataFromFile")]
        public void UploadDataSocContract([FromBody] FriendDTO[] friends)
        {
            
        }
        
    }
}
