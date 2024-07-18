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
        private readonly DataModelValidator _validator;

        public API(IoLabsContext context, DataModelValidator validator)
        {
            _context = context;
            _validator = validator;
        }

        [HttpPost("SaveMessage")]
        public IActionResult SaveMessage(string request)
        {
            var dataModel = new DataModel();
            dataModel.RequestTime = DateTime.Now;
            dataModel.Request = request;
            dataModel.RefreshToken = "r";
            dataModel.AccessToken = "a";
            dataModel.User = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

            // Validation
            var validationResult = _validator.Validate(dataModel);

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
