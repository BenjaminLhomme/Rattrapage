using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Rattrapage.Data;
using Rattrapage.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Rattrapage.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserContext _context;

        public AuthController(UserContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Models.User user)
        {
            var userInDb = _context.Users.FirstOrDefault(u => u.Email == user.Email);

            if (userInDb == null)
            {
                return Unauthorized("Email ou mot de passe incorrect.");
            }

            if (userInDb.Password != user.Password)
            {
                return Unauthorized("Email ou mot de passe incorrect.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("a3R9j$#a2r8j$3j4k5l6m7n8o9p0q1r2s3t4u5v6w7x8y9z0A1B2C3D4E5F");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userInDb.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString });
        }
    }
}
