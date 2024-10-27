using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApp_vSem4.Abstraction;
using WebApp_vSem4.DTO;

namespace WebApp_vSem4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController(IConfiguration _configuration, IUserAuthenticationService _userAuthentication) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] LoginDTO loginDTO)
        {
            var user = _userAuthentication.Authenticate(loginDTO);
            if (user != null)
            {
                var token = GenerateJwtToken(user);

                return Ok(token);
            }
            return NotFound("User not found!");
        }

        private string GenerateJwtToken(UserDTO user)
        {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var tokenMeneger = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier, user.Name!),
                 new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: tokenMeneger);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
