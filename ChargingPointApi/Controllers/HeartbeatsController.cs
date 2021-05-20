using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChargingPointApi.Data;
using ChargingPointApi.Models;
using System.Threading;
using System.Timers;
using System.Net.Mail;
using System.Net;

namespace ChargingPointApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeartbeatsController : ControllerBase
    {
        private readonly SqlDbContext _context;

        public HeartbeatsController(SqlDbContext context)
        {
            _context = context;
        }

        // GET: api/Heartbeats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Heartbeat>>> GetHeartbeats()
        {
            return await _context.Heartbeats.ToListAsync();
        }

        // GET: api/Heartbeats/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Heartbeat>> GetHeartbeat(int id)
        {
            var heartbeat = await _context.Heartbeats.FindAsync(id);

            if (heartbeat == null)
            {
                return NotFound();
            }

            return heartbeat;
        }

        // PUT: api/Heartbeats
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{CPsocket}")]
        public async Task<IActionResult> PutHeartbeat(string CPsocket)
        {

            var CP = await _context.ChargingPoints.FirstOrDefaultAsync(x => x.SocketId == CPsocket);

            var HB = await _context.Heartbeats.FirstOrDefaultAsync(x => x.Cpid == CP.Id);

            if (HB == null)
            {
                _context.Heartbeats.Add(new Heartbeat {Hbtime = DateTime.Now.AddHours(2), Cpid = CP.Id, Cp = CP   });
                
            }
            else
            {
                HB.Hbtime = DateTime.Now.AddHours(2);
                _context.Entry(HB).State = EntityState.Modified;
            }
            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Heartbeats
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Heartbeat>> PostHeartbeat(Heartbeat heartbeat)
        {

            _context.Heartbeats.Add(heartbeat);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHeartbeat", new { id = heartbeat.Id }, heartbeat);
        }



        [HttpGet("/error/{id}")]
        public async Task<IActionResult> SendEmail(int id)
        {

            try
            {

                var CP = await _context.Heartbeats.FindAsync(id);

                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("kenkataservice@gmail.com", "bxieecxvpcowbnsl");


                    MailMessage mess = new MailMessage();
                    mess.To.Add("smode.jesper.m@gmail.com");
                    mess.From = new MailAddress("smode.mattias.e@gmail.com");
                    mess.Subject = $"Charging point is offline | Id:{CP.Id}";
                    mess.Body = $"Charging point is offline | Id:{CP.Id} \nLast Heartbeat was received at: {CP.Hbtime}";
                    client.Send(mess);

                }
            }
            catch
            {
                return new BadRequestResult();
            }

            return new OkResult();
        }

      




        // DELETE: api/Heartbeats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHeartbeat(int id)
        {
            var heartbeat = await _context.Heartbeats.FindAsync(id);
            if (heartbeat == null)
            {
                return NotFound();
            }

            _context.Heartbeats.Remove(heartbeat);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HeartbeatExists(int id)
        {
            return _context.Heartbeats.Any(e => e.Id == id);
        }
    }
}
