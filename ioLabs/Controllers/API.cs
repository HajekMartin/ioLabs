using ioLabs.Data;
using ioLabs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ioLabs.Controllers
{
    [Authorize]
    [ApiController]
    public class API : ControllerBase
    {
        private readonly IoLabsContext _context;

        public API(IoLabsContext context)
        {
            _context = context;
        }

        [HttpPost("SaveMessage")]
        public IActionResult SaveMessage()
        {
            /*
            _context.DataModels.Add(dataModel);
            _context.SaveChanges();
            return Ok(dataModel);
            */
            return Ok();
        }

        [HttpGet("GetMessages")]
        public IActionResult GetMessages()
        {
            var output = _context.DataModels.ToList();
            return Ok(output);
        }
    }
}
