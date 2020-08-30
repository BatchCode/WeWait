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
    public class ConfirmbookingsController : ControllerBase
    {
        private readonly AMCDbContext _context;

        public ConfirmbookingsController(AMCDbContext context)
        {
            _context = context;
        }

        // GET: api/Confirmbookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Confirmbooking>>> GetConfirmbooking()
        {
            return await _context.Confirmbooking.ToListAsync();
        }

        // GET: api/Confirmbookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Confirmbooking>> GetConfirmbooking(int id)
        {
            var confirmbooking = await _context.Confirmbooking.FindAsync(id);

            if (confirmbooking == null)
            {
                return NotFound();
            }

            return confirmbooking;
        }

        // PUT: api/Confirmbookings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConfirmbooking(int id, Confirmbooking confirmbooking)
        {
            if (id != confirmbooking.Id)
            {
                return BadRequest();
            }

            _context.Entry(confirmbooking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConfirmbookingExists(id))
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

        // POST: api/Confirmbookings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Confirmbooking>> PostConfirmbooking(Confirmbooking confirmbooking)
        {
            _context.Confirmbooking.Add(confirmbooking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConfirmbooking", new { id = confirmbooking.Id }, confirmbooking);
        }

        // DELETE: api/Confirmbookings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Confirmbooking>> DeleteConfirmbooking(int id)
        {
            var confirmbooking = await _context.Confirmbooking.FindAsync(id);
            if (confirmbooking == null)
            {
                return NotFound();
            }

            _context.Confirmbooking.Remove(confirmbooking);
            await _context.SaveChangesAsync();

            return confirmbooking;
        }

        private bool ConfirmbookingExists(int id)
        {
            return _context.Confirmbooking.Any(e => e.Id == id);
        }
    }
}
