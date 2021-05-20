using ChargingPointApi.Data;
using ChargingPointApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using Newtonsoft.Json;
using System.Text;

namespace ChargingPointApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckCPController : ControllerBase
    {
        
        private readonly SqlDbContext _context;

        public CheckCPController(SqlDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<bool>> GetCP(string id)
        {
            var CP = await _context.ChargingPoints.FirstOrDefaultAsync(x => x.SocketId == id);

            if (CP == null)
            {
                return false;
            }
            
            return true;
        }


        

    }
}
