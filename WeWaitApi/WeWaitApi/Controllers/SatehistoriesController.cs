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
    public class SatehistoriesController : ControllerBase
    {
        private readonly AMCDbContext _context;

        public SatehistoriesController(AMCDbContext context)
        {
            _context = context;
        }

        // GET: api/Satehistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Satehistory>>> GetSatehistory()
        {
            return await _context.Satehistory.ToListAsync();
        }

        // GET: api/Satehistories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Satehistory>> GetSatehistory(int id)
        {
            var satehistory = await _context.Satehistory.FindAsync(id);

            if (satehistory == null)
            {
                return NotFound();
            }

            return satehistory;
        }

        // PUT: api/Satehistories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSatehistory(int id, Satehistory satehistory)
        {
            if (id != satehistory.StateId)
            {
                return BadRequest();
            }

            _context.Entry(satehistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SatehistoryExists(id))
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

        // POST: api/Satehistories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Satehistory>> PostSatehistory(Satehistory satehistory)
        {
            _context.Satehistory.Add(satehistory);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SatehistoryExists(satehistory.StateId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSatehistory", new { id = satehistory.StateId }, satehistory);
        }

        // DELETE: api/Satehistories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Satehistory>> DeleteSatehistory(int id)
        {
            var satehistory = await _context.Satehistory.FindAsync(id);
            if (satehistory == null)
            {
                return NotFound();
            }

            _context.Satehistory.Remove(satehistory);
            await _context.SaveChangesAsync();

            return satehistory;
        }

        private bool SatehistoryExists(int id)
        {
            return _context.Satehistory.Any(e => e.StateId == id);
        }
    }
}
