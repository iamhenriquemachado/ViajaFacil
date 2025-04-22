using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViajaFacil.Data;
using ViajaFacil.Models.Destines;

namespace ViajaFacil.Controllers.Destines {
    [Route("api/destines/[controller]")]
    [ApiController]
    public class ListDestinesController : ControllerBase {

        private readonly AppDbContext _context;
        private readonly Helpers.Helpers _helpers; 

        public ListDestinesController(AppDbContext context, Helpers.Helpers helpers) {
            _context = context;
            _helpers = helpers;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ListDestines() {

            var userId = _helpers.GetUserIdFromClaims();
            if (userId == null)
                return Unauthorized(new { message = "Unauthorized" });

            var destines = await _context.Destines
                .Select(d => new DestineDTO {
                    Id = d.Id,
                    Name = d.Name, 
                    Description = d.Description,
                    Country = d.Country,
                    State = d.State
                }).ToListAsync();

            return Ok(destines);
        }
    }
}
