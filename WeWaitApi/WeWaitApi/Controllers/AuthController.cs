using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WeWaitApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace WeWaitApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AMCDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AMCDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        private string GenerateJwtToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("RoleId", user.RoleId.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"])); //Get Key JWT & Convert Byte
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512); //Hash JWT

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(120),
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenJwt = tokenHandler.WriteToken(token);

            return tokenJwt; // token
        }

        // /api/Auth/User
        [HttpPost("User")]
        public async Task<ActionResult> Login([FromBody] User user)
        {
            ActionResult response = Unauthorized();

            var usr = await _context.User.FirstOrDefaultAsync(u => (u.Email == user.Email) && (u.Password == user.Password));

            if (usr == null)
            {
                return response;
            }

            var token = GenerateJwtToken(usr);
            response = Ok(new { Token = token, Message = "Success" });

            return response;
        }

        // /api/Auth/Wewaiter
        [HttpPost("Wewaiter")]
        public async Task<ActionResult> LoginWewaiter([FromBody] User user)
        {
            ActionResult response = Unauthorized();

            var usr = await _context.User.FirstOrDefaultAsync(u => (u.Email == user.Email) && (u.Password == user.Password) && (u.RoleId == 2));

            if (usr == null)
            {
                return response;
            }

            var token = GenerateJwtToken(usr);
            response = Ok(new { Token = token, Message = "Success" });

            return response;
        }

    }
}
