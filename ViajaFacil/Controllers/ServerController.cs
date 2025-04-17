using Microsoft.AspNetCore.Mvc;

namespace ViajaFacil.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase {

        [HttpGet("check")]
        public IActionResult Index() {
            return new JsonResult(new {
                message = "Server is running!",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
