using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeWaitApi.Models;

namespace WeWaitApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventbookingsController : ControllerBase
    {
        private readonly AMCDbContext _context;

        public EventbookingsController(AMCDbContext context)
        {
            _context = context;
        }

        // GET: api/Eventbookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Eventbooking>>> GetEventbooking()
        {
            return await _context.Eventbooking.ToListAsync();
        }

        // GET: api/Eventbookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Eventbooking>> GetEventbooking(int id)
        {
            var eventbooking = await _context.Eventbooking.FindAsync(id);

            if (eventbooking == null)
            {
                return NotFound();
            }

            return eventbooking;
        }

        // PUT: api/Eventbookings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventbooking(int id, Eventbooking eventbooking)
        {
            if (id != eventbooking.EventId)
            {
                return BadRequest();
            }

            _context.Entry(eventbooking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventbookingExists(id))
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

        // POST: api/Eventbookings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Eventbooking>> PostEventbooking(Eventbooking eventbooking)
        {
            _context.Eventbooking.Add(eventbooking);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EventbookingExists(eventbooking.EventId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEventbooking", new { id = eventbooking.EventId }, eventbooking);
        }

        // DELETE: api/Eventbookings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Eventbooking>> DeleteEventbooking(int id)
        {
            var eventbooking = await _context.Eventbooking.FindAsync(id);
            if (eventbooking == null)
            {
                return NotFound();
            }

            _context.Eventbooking.Remove(eventbooking);
            await _context.SaveChangesAsync();

            return eventbooking;
        }

        private bool EventbookingExists(int id)
        {
            return _context.Eventbooking.Any(e => e.EventId == id);
        }
    }
}
