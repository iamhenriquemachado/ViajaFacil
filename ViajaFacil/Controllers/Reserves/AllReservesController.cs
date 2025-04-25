using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViajaFacil.Data;
using Microsoft.EntityFrameworkCore;
using ViajaFacil.Models.Reserves;

namespace ViajaFacil.Controllers.Reserves {
    [Route("api/reserves/[controller]")]
    [ApiController]
    public class AllReservesController : ControllerBase {

        private readonly AppDbContext _context;
        private readonly Helpers.Helpers _helpers;

        public AllReservesController(AppDbContext context, Helpers.Helpers helpers) {
            _context = context;
            _helpers = helpers;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ListAllReserves() {

            var loggedInUser = _helpers.GetUserIdFromClaims();
            if (loggedInUser == null)
                return Unauthorized(new { message = "Unauthorized" });

            if (!await _helpers.IsAdminUser(loggedInUser.Value))
                return Unauthorized(new { message = "Only administrators are allowed list all reserves" });

            var allReserves = await _context.Reserves
                .Select(r => new ReserveModel {
                    Id = r.Id,
                    UserId = r.UserId,
                    DestinationId = r.DestinationId,
                    ReservationDate = r.ReservationDate,
                    Status = r.Status
                }).ToListAsync();

            return Ok(allReserves);
        }
    }
}
