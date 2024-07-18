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
            //dataModel.RefreshToken = ...;
            //dataModel.AccessToken = ...;
            

            // Validace
            var validator = new DataModelValidator();
            var validationResult = validator.Validate(dataModel);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            _context.DataModels.Add(dataModel);
            _context.SaveChanges();
            return Ok(dataModel);
        }

        [HttpGet("GetMessages")]
        public IActionResult GetMessages()
        {
            var output = _context.DataModels.ToList();
            return Ok(output);
        }
    }
}
