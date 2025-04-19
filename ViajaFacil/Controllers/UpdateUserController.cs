using System.Data.Entity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViajaFacil.Data;
using ViajaFacil.Models; 

namespace ViajaFacil.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateUserController : ControllerBase {
        private readonly AppDbContext _context;
        public UpdateUserController(AppDbContext context) {
            _context = context;
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUserById(int id, [FromBody] UserUpdateDTO data) {

            if (!ModelState.IsValid) {
                var errors = ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => new {
                        Field = e.Key,
                        Errors = e.Value.Errors.Select(err => err.ErrorMessage)
                    });

                return BadRequest(new {
                    message = "Invalid request or user not found",
                    errors
                });
            }


            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) 
                return Unauthorized(new { message = "Unauthorized" });

            if (!int.TryParse(userId, out int adminId))
                return Unauthorized(new { message = "Invalid user ID" });

            if (data == null)
                return BadRequest(new { message = "No data provided" });

            var userAdmin = await _context.Users.FindAsync(int.Parse(userId));
            if (userAdmin == null || !userAdmin.IsAdmin) 
                return Unauthorized(new { message = "Only administrators are allowed to perform this action" });

            var user = await _context.Users.FindAsync(id);
            if (user == null) 
                return NotFound(new { message = "User not found" });

            bool emailExists = _context.Users.Any(u => u.Email == data.Email.Trim().ToLowerInvariant() && u.Id != id);
            if (emailExists)
                return Conflict(new { message = "Email is already in use by another account." });

            user.Name = data.Name;
            user.Email = data.Email.Trim().ToLowerInvariant();
            user.LastModified = DateTime.UtcNow;
     
            await _context.SaveChangesAsync();

            return Ok(new { message = "User updated successfully" });
        }

    }
}
