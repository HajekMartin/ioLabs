using ioLabs.Services;
using Microsoft.AspNetCore.Mvc;

namespace ioLabs.Controllers
{
    public class TokenController : Controller
    {
        private readonly TokenService _tokenService;

        public TokenController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("refresh-token")]
        public async Task<IActionResult> GetSecureData(string refreshToken)
        {
            // Refresh the access token
            var tokenResponse = await _tokenService.RefreshAccessTokenAsync(refreshToken);
            if (tokenResponse != null)
            {
                Console.WriteLine(tokenResponse.AccessToken);
                Console.WriteLine(tokenResponse.RefreshToken);

                return Ok(tokenResponse); // Aplikace už pak nadále s tím naloží sama
            }
            else
            {
                return Unauthorized("Failed to refresh token.");
            }
        }
    }
}
