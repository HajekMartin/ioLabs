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

        [HttpGet("tokens")]
        public async Task<IActionResult> GetSecureData(string refreshToken)
        {
            // Refresh the access token
            var tokenResponse = await _tokenService.RefreshAccessTokenAsync(refreshToken);
            if (tokenResponse != null)
            {
                var accessToken = tokenResponse.AccessToken;

                return Ok(accessToken);
            }
            else
            {
                // Handle token refresh failure
                return Unauthorized("Failed to refresh token.");
            }
        }
    }
}
