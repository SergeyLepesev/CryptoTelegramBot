using Microsoft.AspNetCore.Mvc;

namespace CryptoBot.Controllers
{
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        public IActionResult Check()
        {
            return Ok(new
            {
                Status = "ok"
            });
        }
    }
}