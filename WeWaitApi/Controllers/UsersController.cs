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
    /*[Authorize]*/
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AMCDbContext _context;

        public UsersController(AMCDbContext context)
        {
            _context = context;
        }

        // GET: api/Users/GetByEmail (body parameter : email)
        [HttpGet("GetByEmail")]
        public async Task<ActionResult<User>> GetUserByEmail([FromBody] User user)
        {
            /* How to perform Raw SQL query */
            // Build the query
            var email = user.Email;
            string query = $"SELECT * FROM USER WHERE Email = @email";
            var p1 = new MySqlParameter("@email", email);
            
            // Execute
            var usr = await _context.User.FromSqlRaw(query, p1).AsNoTracking().FirstOrDefaultAsync();

            /* Or using the fashion way */
            //  var usr = await _context.User.FirstOrDefaultAsync(u => ((u.Email == user.Email) && (u.Password == user.Password)));

            if (usr == null)
            {
                return NotFound();
            }

            return usr;
        }

        // GET: api/GetAllByRole/1
        [HttpGet("GetAllByRole/{id}")]
        public async Task<ActionResult<List<User>>> GetAllUsersByRole(int id)
        {
            return await _context.User.Where(u => u.RoleId == id).ToListAsync();
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            // int roleId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "RoleId").Value);
            // if (roleId == 2) { return NotFound(new { Message = "You are not the admin!" }); }

            return await _context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var usr = await _context.User.FirstOrDefaultAsync(u => u.Email == user.Email);

            // Check if user already exists
            if (usr != null) {
                // return Deny("User already exists");
                return StatusCode(409);
            }

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
