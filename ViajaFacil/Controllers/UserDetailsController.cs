using Microsoft.AspNetCore.Mvc;
using ViajaFacil.Data;
using ViajaFacil.Helpers; 

namespace ViajaFacil.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase {

        private readonly AppDbContext _context;
        private readonly Helpers.Helpers _helpers; 
        public UserDetailsController(AppDbContext context, Helpers.Helpers helpers) {
            _context = context;
            _helpers = helpers;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDetails(int id) {

            var user = await _helpers.GetUserById(id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            // Retrive user data by ID 
            return Ok(new {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                isAdmin = user.IsAdmin
            });
        }
    }
}
