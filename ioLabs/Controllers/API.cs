using Microsoft.AspNetCore.Mvc;

namespace ioLabs.Controllers
{
    public class API : ControllerBase
    {
        [HttpPost("SaveMessage")]
        public IActionResult SaveNMessage()
        {
            return Ok();
        }

        [HttpGet("GetMessages")]
        public IActionResult GetMessages()
        {
            return Ok();
        }
    }
}
