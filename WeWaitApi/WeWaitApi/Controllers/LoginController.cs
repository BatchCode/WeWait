using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeWaitApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeWaitApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AMCDbContext _context;

        public LoginController(AMCDbContext context)
        {
            _context = context;
        } // Dependency injection (DB) 

        [HttpPost]
        public async Task<User> Post([FromBody] User usr)
        {


            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == usr.Email && u.Password == usr.Password);
            // var usr = await _context.User.Where(u => u.Email == user.Email).ToListAsync();
            
            
            return user;


        }
    }
}
