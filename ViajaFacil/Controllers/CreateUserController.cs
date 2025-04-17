using Microsoft.AspNetCore.Mvc;
using ViajaFacil.Data;
using ViajaFacil.Models;
using Microsoft.AspNetCore.Identity; 
using Microsoft.EntityFrameworkCore;
using System;

namespace ViajaFacil.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CreateUserController : ControllerBase {
        private readonly AppDbContext _context;

        public CreateUserController(AppDbContext context) {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var passwordHasher = new PasswordHasher<User>();
            user.Password = passwordHasher.HashPassword(user, user.Password);

            var checkUserExists = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);
            if (checkUserExists != null) {
                return Conflict(new { error = "Email already in use.", StatusCode = 409 });
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { user = "User created successfully", user.Id, user.Name });
        }
    }
}
