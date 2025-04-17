using Microsoft.AspNetCore.Mvc;
using ViajaFacil.Services;
using ViajaFacil.Models;
using ViajaFacil.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net; 


namespace ViajaFacil.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase {

        private readonly ITokenService _tokenService;
        private readonly AppDbContext _context; 

        public AuthenticationController(ITokenService tokenService, AppDbContext context) {
            _tokenService = tokenService;
            _context = context; 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model) {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password)) {
                return BadRequest(new { message = "Username and password are required." });
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == model.Username);

            if (user == null) {
                return Unauthorized(new { message = "Invalid email or password" });
            };

            // Check password
            bool validPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);
            if (!validPassword) {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var token = _tokenService.GenerateToken(
               user.Id.ToString(),
               user.Email, 
               user.IsAdmin
           );

            return Ok(new {
                token,
                user = new {
                    user.Id,
                    user.Name,
                    user.Email,
                    user.IsAdmin
                }
            });
        }

    }
}
