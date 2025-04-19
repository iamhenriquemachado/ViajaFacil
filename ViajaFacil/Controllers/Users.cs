using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViajaFacil.Data;
using ViajaFacil.Models;


namespace ViajaFacil.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class Users : ControllerBase {

        private readonly AppDbContext _context;
        public Users(AppDbContext context) {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers() {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { message = "Unauthorized" });

            var userAdmin = await _context.Users.FindAsync(int.Parse(userId));
            if (userAdmin == null || !userAdmin.IsAdmin)
                return Unauthorized(new { message = "Only administrators are allowed to perform this action" });

            var users = await _context.Users
                    .Select(u => new UserDTO {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email
                    }).ToListAsync();

            return Ok(users); 
        }

    }
}
