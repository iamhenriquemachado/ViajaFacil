using Microsoft.AspNetCore.Mvc;
using ViajaFacil.Data;

namespace ViajaFacil.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase {

        private readonly AppDbContext _context;
        public UserDetailsController(AppDbContext context) {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDetails(int id) {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(new {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                isAdmin = user.IsAdmin
            });
        }
    }
}
