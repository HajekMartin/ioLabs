using ioLabs.Data;
using ioLabs.Models;
using ioLabs.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ioLabs.Controllers
{
    [Authorize]
    [ApiController]
    public class APISql : Controller
    {
        private readonly IoLabsSQLContext _contextSql;
        private readonly DataModelValidator _validator;

        public APISql(IoLabsSQLContext contextSql, DataModelValidator validator)
        {
            _contextSql = contextSql;
            _validator = validator;
        }


        [HttpPost("SaveMessageSql")]
        public IActionResult SaveMessageSql(string request)
        {
            var dataModel = new DataModel();
            dataModel.RequestTime = DateTime.Now;
            dataModel.Request = request;
            dataModel.RefreshToken = "refresh";
            dataModel.AccessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            dataModel.User = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

            // Validation
            var validationResult = _validator.Validate(dataModel);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            _contextSql.DataModels.Add(dataModel);
            _contextSql.SaveChanges();

            return Ok("You send: " + dataModel.Request);
        }

        [HttpGet("GetMessagesSql")]
        public IActionResult GetMessagesSql(int page, int pageCount, string search)
        {
            var query = _contextSql.DataModels.AsQueryable();
            if (!search.IsNullOrEmpty())
            {
                query = query.Where(dm => dm.Request.Contains(search));
            }

            var output = query
                .Skip((page - 1) * pageCount)
                .Take(pageCount)
                .ToList();

            return Ok(output);
        }
    }
}
