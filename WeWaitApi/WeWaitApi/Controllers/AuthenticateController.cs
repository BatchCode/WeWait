using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Web.Services3.Security.Utility;
using WeWaitApi.Models;

namespace WeWaitApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]

    public class AuthenticateController : ControllerBase
    {

        #region Propriete
        private IConfiguration _config;
        private readonly AMCDbContext _context;
        #endregion

        #region Constructeur
        public AuthenticateController(IConfiguration config, AMCDbContext context)
        {
            _config = config;
            _context = context;
        }
        #endregion

        #region GenerateJWT
        //Methode Generate Token
        private string GenerateJSONWebToken(User user)
        {
            //Parametre renseigner Token JWT
            List<Claim> claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("RoleId", user.RoleId.ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])); //Get Key JWT & Convert Byte
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); //Hash JWT

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _config["Jwt:Issuer"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(120), //Expire 2H
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);            

            return tokenHandler.WriteToken(token);
        }
        #endregion

        #region AuthenticateUser
        //Validation Connexion
        private async Task<User> AuthenticateUser(LoginModel usr)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => ((u.Email == usr.Email) && (u.Password == usr.Password)));
            if (user == null)
            {
                return null;
            }
            return user;


        }
        #endregion

        #region Login Validation  
        /// <summary>  
        /// Login Authenticaton using JWT Token Authentication  
        /// </summary>  
        /// <param name="data"></param>  
        /// <returns></returns>  
        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] LoginModel data)
        {
            IActionResult response = Unauthorized();
            var user = await AuthenticateUser(data);
            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { Token = tokenString, Message = "Success" });
            }
            return response;
        }
        #endregion

        #region Get  
        /// <summary>  
        /// Authorize the Method  
        /// </summary>  
        /// <returns></returns>  
        [HttpGet(nameof(Get))]
        public async Task<IEnumerable<string>> Get()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            return new string[] { accessToken };
        }
        #endregion
    }


    #region JsonProperties
    public class LoginModel
        {
            [Required]
            public string Email { get; set; }
            [Required]
            public string Password { get; set; }
        }
        #endregion
}

