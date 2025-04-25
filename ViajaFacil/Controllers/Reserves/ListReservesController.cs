using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViajaFacil.Data;

namespace ViajaFacil.Controllers.Reserves {
    [Route("api/reserves/[controller]")]
    [ApiController]
    public class ListReservesController : ControllerBase {

        private readonly AppDbContext _context;
        private readonly Helpers.Helpers _helpers;

        public ListReservesController(AppDbContext context, Helpers.Helpers helpers) {
            _context = context;
            _helpers = helpers;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ListReserves() {
            var loggedInUserId = _helpers.GetUserIdFromClaims();
            if (loggedInUserId == null)
                return Unauthorized(new { message = "Unauthorized" });

            
            var findUserReserves = await _context.Reserves
                                        .Where(r => r.UserId == loggedInUserId)
                                        .ToListAsync();

            return Ok(new {
                listReserves = findUserReserves,
            });
        }
    }
}

