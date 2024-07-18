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
                // Use the new access token to make your secure API call
                var accessToken = tokenResponse.AccessToken;

                // Optionally, store the new access and refresh tokens securely

                // Proceed with your API logic using the new access token
                return Ok("Secure data accessed using refreshed token.");
            }
            else
            {
                // Handle token refresh failure
                return Unauthorized("Failed to refresh token.");
            }
        }
    }
}
