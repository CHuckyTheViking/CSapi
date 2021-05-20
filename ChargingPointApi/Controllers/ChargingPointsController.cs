using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChargingPointApi.Data;
using ChargingPointApi.Models;

namespace ChargingPointApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChargingPointsController : ControllerBase
    {
        private readonly SqlDbContext _context;

        public ChargingPointsController(SqlDbContext context)
        {
            _context = context;
        }

        // GET: api/ChargingPoints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChargingPoint>>> GetChargingPoints()
        {
            return await _context.ChargingPoints.ToListAsync();
        }

        // GET: api/ChargingPoints/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChargingPoint>> GetChargingPoint(int id)
        {
            var chargingPoint = await _context.ChargingPoints.FindAsync(id);

            if (chargingPoint == null)
            {
                return NotFound();
            }

            return chargingPoint;
        }

        // PUT: api/ChargingPoints/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChargingPoint(int id, ChargingPoint chargingPoint)
        {
            if (id != chargingPoint.Id)
            {
                return BadRequest();
            }

            _context.Entry(chargingPoint).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChargingPointExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ChargingPoints
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChargingPoint>> PostChargingPoint(ChargingPoint chargingPoint)
        {
            _context.ChargingPoints.Add(chargingPoint);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChargingPoint", new { id = chargingPoint.Id }, chargingPoint);
        }

        // DELETE: api/ChargingPoints/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChargingPoint(int id)
        {
            var chargingPoint = await _context.ChargingPoints.FindAsync(id);
            if (chargingPoint == null)
            {
                return NotFound();
            }

            _context.ChargingPoints.Remove(chargingPoint);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}")]
        



        private bool ChargingPointExists(int id)
        {
            return _context.ChargingPoints.Any(e => e.Id == id);
        }
    }

    
}
