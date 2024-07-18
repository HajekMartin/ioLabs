using Newtonsoft.Json;

namespace ioLabs.Services
{
    public class TokenService
    {
        private readonly HttpClient _httpClient;

        public TokenService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TokenResponse> RefreshAccessTokenAsync(string refreshToken)
        {
            var tokenEndpoint = "https://keycloak.stage.iolabs.ch/auth/realms/iotest/protocol/openid-connect/token";
            var clientId = "test-api";
            var clientSecret = "vCDbLdj631sATkfujdg75j9WGzafryKf";

            var tokenRequestContent = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("client_id", clientId),
                    new KeyValuePair<string, string>("client_secret", clientSecret),
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                    new KeyValuePair<string, string>("refresh_token", refreshToken),
                });

            var tokenResponse = await _httpClient.PostAsync(tokenEndpoint, tokenRequestContent);
            if (tokenResponse.IsSuccessStatusCode)
            {
                var jsonContent = await tokenResponse.Content.ReadAsStringAsync();
                var tokenResponseObject = JsonConvert.DeserializeObject<TokenResponse>(jsonContent);
                return tokenResponseObject;
            }

            return null; // Or handle errors appropriately
        }
    }

    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
