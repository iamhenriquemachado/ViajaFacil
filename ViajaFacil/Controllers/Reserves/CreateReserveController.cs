using System.Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViajaFacil.Data;
using ViajaFacil.Models.Reserves;

namespace ViajaFacil.Controllers.Reserves {
    [Route("api/reserves/[controller]")]
    [ApiController]
    public class CreateReserveController : ControllerBase {

        private readonly AppDbContext _context;
        private readonly Helpers.Helpers _helpers;

        public CreateReserveController(AppDbContext context, Helpers.Helpers helpers) {
            _context = context;
            _helpers = helpers;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateReserve(ReserveModel reserve) {

            var loggedInUserId = _helpers.GetUserIdFromClaims();
            if (loggedInUserId == null)
                return Unauthorized(new { message = "Unauthorized" });

            var user = await _context.Users.FindAsync(loggedInUserId.Value);
            if (user == null)
                return NotFound(new { message = "User not found" });

            if(user.IsAdmin == true)
                return Unauthorized(new { message = "Only regular users can create reserves" });

            var destination = await _context.Destines.FindAsync(reserve.DestinationId);
             if(destination == null)
                return NotFound(new { message = "Destination not found" });

            var checkDuplicatedReserve = _context.Reserves.FirstOrDefault(r => r.UserId == loggedInUserId.Value && r.DestinationId == reserve.DestinationId);
            if (checkDuplicatedReserve != null)
                return Conflict(new { message = "You cannot create two reserves for the same destination" });

            reserve.UserId = loggedInUserId.Value;
            reserve.Status = "Confirmed";

            // Save new reserve in the database
            _context.Reserves.Add(reserve);
            await _context.SaveChangesAsync();

            // Add your logic here for creating a reserve
            return Ok(new { message = "Reserve created successfully", reserveId = reserve.Id });
        }
    }
}
