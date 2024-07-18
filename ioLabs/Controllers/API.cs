using ioLabs.Data;
using ioLabs.Models;
using ioLabs.Validators;
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
        public IActionResult SaveMessage(string request)
        {
            var dataModel = new DataModel();
            dataModel.RequestTime = DateTime.Now;
            dataModel.Request = request;
            dataModel.RefreshToken = "r";
            dataModel.AccessToken = "a";
            dataModel.User = "u";

            // Validation
            var validator = new DataModelValidator();
            var validationResult = validator.Validate(dataModel);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            _context.DataModels.Add(dataModel);
            _context.SaveChanges();
            return Ok("You send: " + dataModel.Request);
        }

        [HttpGet("GetMessages")]
        public IActionResult GetMessages(int page, int pageCount)
        {
            var output = _context.DataModels
                .Skip((page - 1) * pageCount)
                .Take(pageCount)
                .ToList();
            return Ok(output);
        }
    }
}
