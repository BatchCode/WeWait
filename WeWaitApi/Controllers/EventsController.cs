using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using WeWaitApi.Models;

namespace WeWaitApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly AMCDbContext _context;

        public EventsController(AMCDbContext context)
        {
            _context = context;
        }


        // GET: api/Event/GetByCategory (body parameter : Category)
        [HttpGet("GetByCategory")]
        public async Task<ActionResult<List<Event>>> GetByCategory([FromBody] Event @event)
        {
            /* How to perform Raw SQL query */
            // Build the query
            var category = @event.Category;
            string query = $"SELECT * FROM EVENT WHERE CATEGORY = @category";
            var p1 = new MySqlParameter("@category", category);

            // Execute
            var qevent = await _context.Event.FromSqlRaw(query, p1).AsNoTracking().ToListAsync();

            /* Or using the fashion way */
            //  var usr = await _context.User.FirstOrDefaultAsync(u => ((u.Email == user.Email) && (u.Password == user.Password)));

            if (qevent == null)
            {
                return NotFound();
            }

            return qevent;
        }


        // GET: api/GetAllByRole/1
        [HttpGet("GetAllByRole/{id}")]
        public async Task<ActionResult<List<Event>>> GetAllEventById(int id)
        {
            return await _context.Event.Where(e => e.Id == id).ToListAsync();
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvent()
        {
            return await _context.Event.ToListAsync();
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var @event = await _context.Event.FindAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {
            ActionResult response = Unauthorized();

            if (id != @event.Id)
            {
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            response = Ok(new {Message = "Sucess update" });
            return response;
        }

        // POST: api/Events/PostEvents
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("PostEvents")]
        public async Task<ActionResult<Event>> PostEvent(Event @event)
        {
            var evnt = await _context.Event.FirstOrDefaultAsync(e =>(e.Name == @event.Name) && (e.Category == @event.Category) && (e.Actor == @event.Actor) && (e.Seats == @event.Seats) && (e.DateStart == @event.DateStart) && (e.DateEnd == @event.DateEnd));

            if (evnt != null)
            {
                return StatusCode(409);
            }

            _context.Event.Add(@event);
            await _context.SaveChangesAsync();
        
            return CreatedAtAction("GetRole", new {id = @event.Id }, @event);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Event>> DeleteEvent(int id)
        {
            ActionResult response = Unauthorized();

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            _context.Event.Remove(@event);
            await _context.SaveChangesAsync();

            response = Ok(new { Message = "Event DELETE :", @event });
            return response;
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.Id == id);
        }
    }
}
