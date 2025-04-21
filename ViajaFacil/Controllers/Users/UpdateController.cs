using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViajaFacil.Data;
using ViajaFacil.Helpers;
using ViajaFacil.Models;

namespace ViajaFacil.Controllers.Users {
    [Route("api/users/[controller]")]
    [ApiController]
    public class UpdateController : ControllerBase {
        private readonly AppDbContext _context;
        private readonly Helpers.Helpers _helpers; 

        public UpdateController(AppDbContext context, Helpers.Helpers helpers) {
            _context = context;
            _helpers = helpers;
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUserById(int id, [FromBody] UserUpdateDTO data) {

            var modelError = this.GetModelStateErrorResponse();
            if (modelError != null)
                return modelError;

            var userId = _helpers.GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized(new { message = "Unauthorized" });

            if (!await _helpers.IsAdminUser(userId.Value))
                return Unauthorized(new { message = "Only administrators are allowed" });

            var nullDataResponse = this.GetNullDataResponse(data);
            if (nullDataResponse != null) 
                return nullDataResponse;

            var user = await _helpers.GetUserById(id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            if (await _helpers.EmailExistsForAnotherUser(data.Email, id))
                return Conflict(new { message = "Email already in use by another account"});


            // Update user data
            user.Name = data.Name;
            user.Email = data.Email.Trim().ToLowerInvariant();
            user.IsAdmin = data.IsAdmin; 
            user.LastModified = DateTime.UtcNow;

            // Save uuser updated
            await _context.SaveChangesAsync();

            return Ok(new { message = "User updated successfully" });
        }
    }
}
