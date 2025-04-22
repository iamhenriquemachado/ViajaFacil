using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViajaFacil.Data;
using Microsoft.EntityFrameworkCore;

namespace ViajaFacil.Controllers.Destines {
    [Route("api/destines/[controller]")]
    [ApiController]
    public class DeleteDestineController : ControllerBase {

        private readonly AppDbContext _context;
        private readonly Helpers.Helpers _helpers;

        public DeleteDestineController(AppDbContext context, Helpers.Helpers helpers) {
            _context = context;
            _helpers = helpers;
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDestine(int id) {

            var loggedInUserId = _helpers.GetUserIdFromClaims();
            if (loggedInUserId == null)
                return Unauthorized(new { message = "Unauthorized" });

            if (!await _helpers.IsAdminUser(loggedInUserId.Value))
                return Unauthorized(new { message = "Only administrators are allowed to delete users" });

            var destineToDelete = await _context.Destines.FirstOrDefaultAsync(d => d.Id == id);
            if (destineToDelete == null)
                return NotFound(new { message = "Destine not found" });

            _context.Destines.Remove(destineToDelete);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Destine deleted successfully" });
        }
    }
}
