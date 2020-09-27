using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeWaitApi.Models;

namespace WeWaitApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AMCDbContext _context;

        public RolesController(AMCDbContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRole()
        {
            return await _context.Role.ToListAsync();
        }

        // GET: api/Roles/Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRoleId(int id)
        {
            var role = await _context.Role.FirstAsync(u => u.Id == id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        // GET: api/GetRoleByLabel/label
        [HttpGet("GetRoleByLabel/{label}")]
        public async Task<ActionResult<Role>> GetRoleLabel(string Label)
        {
            var roleLabel = await _context.Role.FirstOrDefaultAsync(u => u.Label == Label);

            if (roleLabel == null)
            {
                return NotFound();
            }

            return roleLabel;
        }


        // PUT: api/Roles/id
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, Role role)
        {
            if (id != role.Id)
            {
                return BadRequest();
            }

            _context.Entry(role).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
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

        // POST: api/Roles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(Role role)
        {
            //Check en Bdd Role Existant
            var ro = await _context.Role.FirstOrDefaultAsync(r => r.Label == role.Label);
            
            //Check Role Inexistant
            if (ro != null)
            {
                return StatusCode(409);
            }

            _context.Role.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRole", new { id = role.Id }, role);
        }

        // DELETE: api/Roles/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Role>> DeleteRole(int id)
        {
            var role = await _context.Role.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Role.Remove(role);
            await _context.SaveChangesAsync();

            return role;
        }

        private bool RoleExists(int id)
        {
            return _context.Role.Any(e => e.Id == id);
        }
    }
}
