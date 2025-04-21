using Microsoft.AspNetCore.Mvc;
using ViajaFacil.Services;
using ViajaFacil.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ViajaFacil.Models.Users;


namespace ViajaFacil.Controllers.Auth {
    [Route("api/users/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase {

        private readonly ITokenService _tokenService;
        private readonly AppDbContext _context; 

        public AuthenticationController(ITokenService tokenService, AppDbContext context) {
            _tokenService = tokenService;
            _context = context; 
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model) {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password)) {
                return BadRequest(new { message = "Username and password are required." });
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null) {
                return Unauthorized(new { message = "Invalid email or password 01" });
            };


            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);
            if (result == PasswordVerificationResult.Failed) {
                return Unauthorized(new { message = "Invalid email or password 02" });
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

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetUserLoggedData() {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized(new { message = "Unauthorized" });

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null) return NotFound(new { message = "User not found" });

            return Ok(new {
                id = userId,
                name = user.Name,
                email = user.Email,
                isAdmin = user.IsAdmin
            });
        }

    }
}
