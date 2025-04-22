using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViajaFacil.Data;
using ViajaFacil.Models.Destines;
using ViajaFacil.Models.Users;

namespace ViajaFacil.Controllers.Destines {
    [Route("api/destines/[controller]")]
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
        public async Task<IActionResult> UpdateDestine(int id, [FromBody] DestineUpdateDTO data) {

            var userId = _helpers.GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized(new { message = "Unauthorized" });

            if (!await _helpers.IsAdminUser(userId.Value))
                return Unauthorized(new { message = "Only administrators are allowed" });

            var destine = await _helpers.GetDestineById(id);
            if (destine == null)
                return NotFound(new { message = "User not found" }); ;

            destine.Name = data.Name;
            destine.Description = data.Description;
            destine.ImageUrl = data.ImageUrl;
            destine.Country = data.Country;
            destine.State = data.State; 
            destine.City = data.City;
            destine.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Destine updated successfully" });
        }
    }
}
