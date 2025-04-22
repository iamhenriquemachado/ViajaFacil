using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViajaFacil.Data;

namespace ViajaFacil.Controllers.Destines {
    [Route("api/destines/[controller]")]
    [ApiController]
    public class DetailDestineController : ControllerBase {
        private readonly AppDbContext _context;
        private readonly Helpers.Helpers _helpers;

        public DetailDestineController(AppDbContext context, Helpers.Helpers helpers) {
            _context = context;
            _helpers = helpers;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id) {

            var destineId = await _context.Destines.FindAsync(id);
            if (destineId == null) {
                return NotFound(new { error = "Destine not found.", StatusCode = 404 });
            }

            return Ok (new {
                id = destineId.Id,
                name = destineId.Name,
                description = destineId.Description,
                imageUrl = destineId.ImageUrl,
                createdAt = destineId.CreatedAt,
                updatedAt = destineId.UpdatedAt
            });

        }
    }
}
