using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViajaFacil.Data;
using ViajaFacil.Helpers;
using ViajaFacil.Models.Destines;
using Microsoft.EntityFrameworkCore;

namespace ViajaFacil.Controllers.Destines {
    [Route("api/destines/[controller]")]
    [ApiController]
    public class CreateDestineController : ControllerBase {
        private readonly AppDbContext _context;
        private readonly Helpers.Helpers _helpers;

        public CreateDestineController(AppDbContext context, Helpers.Helpers helpers) {
            _context = context;
            _helpers = helpers;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateDestine(DestineModel destine) {
            var modelError = ValidateModel();
            if (modelError != null)
                return modelError;

            if (await DestineExists(destine.Name))
                return Conflict(new { error = "Destine already exists.", StatusCode = 409 });

            return await SaveDestine(destine);
        }

        private IActionResult? ValidateModel() {
            var modelError = this.GetModelStateErrorResponse();
            return modelError;
        }

        private async Task<bool> DestineExists(string name) {
            return await _context.Destines
                .AnyAsync(d => d.Name.ToLower() == name.ToLower());
        }

        private async Task<IActionResult> SaveDestine(DestineModel destine) {
            try {
                _context.Destines.Add(destine);
                await _context.SaveChangesAsync();

                return Ok(new {
                    message = "Destine created successfully",
                    destine.Id,
                    destine.Name
                });
            }
            catch (DbUpdateException ex) {
                return StatusCode(500, new {
                    error = "Database error while saving the destine.",
                    details = ex.Message
                });
            }
        }
    }
}
