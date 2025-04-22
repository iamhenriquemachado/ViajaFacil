using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViajaFacil.Data;
using ViajaFacil.Helpers;

namespace ViajaFacil.Controllers.Users {
    [Route("api/users/[controller]")]
    [ApiController]
    public class DeleteUserController : ControllerBase {

        private readonly AppDbContext _context;
        private readonly Helpers.Helpers _helpers; 

        public DeleteUserController(AppDbContext context, Helpers.Helpers helpers) {
            _context = context;
            _helpers = helpers;
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(int id) {

            var modelError = this.GetModelStateErrorResponse();
            if (modelError != null)
                return modelError;

            var loggedInUserId = _helpers.GetUserIdFromClaims();
            if (loggedInUserId == null)
                return Unauthorized(new { message = "Unauthorized" });

            if (!await _helpers.IsAdminUser(loggedInUserId.Value))
                return Unauthorized(new { message = "Only administrators are allowed to delete users" });

            var userToDelete = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (userToDelete == null)
                return NotFound(new { message = "User not found" });

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully" });

        }
    }
}
