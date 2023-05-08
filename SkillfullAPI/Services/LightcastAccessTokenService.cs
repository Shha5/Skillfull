using Newtonsoft.Json;
using SkillfullAPI.Models.LightcastApiModels;
using SkillfullAPI.Services.Interfaces;
using System;
using System.Text;

namespace SkillfullAPI.Services
{
    public class LightcastAccessTokenService : ILightcastAccessTokenService
    {
        private readonly HttpClient _authClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LightcastAccessTokenService> _logger;

        public LightcastAccessTokenService(HttpClient authClient, IConfiguration configuration, ILogger<LightcastAccessTokenService> logger)
        {
            _authClient = authClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<LightcastAuthTokenModel> GetLightcastTokenAsync()
        {
            string authResponse = await GetAuthResponseAsync();
            LightcastAuthTokenModel token = await DeserializeAuthResponseAsync(authResponse);
            if (token != null)
            {
                return token;
            }
            else
            {
                return null;
            }
        }


        private async Task<string> GetAuthResponseAsync()
        {
            var accessInfo = _configuration.GetSection("LightcastApi");
            if (accessInfo != null)
            {
                string clientId = accessInfo.GetValue<string>("ClientId");
                string secret = accessInfo.GetValue<string>("Secret");
                string scope = accessInfo.GetValue<string>("Scope");

                var values = new Dictionary<string, string>
                {
                    {"client_id", clientId },
                    {"client_secret", secret},
                    {"grant_type", "client_credentials"},
                    {"scope", scope},
                };

                var requestContent = new FormUrlEncodedContent(values);

                var response = await _authClient.PostAsync("https://auth.emsicloud.com/connect/token", requestContent);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    _logger.LogInformation(nameof(LightcastAccessTokenService), $"Request to auth.emsicloud.com/connect/token returned {response.StatusCode}");
                    return null;
                }
            }
            else
            {
                _logger.LogInformation(nameof(LightcastAccessTokenService), $"Couldn't retrieve access information from secrets.json");
                return null;
            }
        }

        private async Task<LightcastAuthTokenModel> DeserializeAuthResponseAsync(string authResponse)
        {
            if (!string.IsNullOrEmpty(authResponse))
            {
                LightcastAuthTokenModel token;
                try
                {
                    token = JsonConvert.DeserializeObject<LightcastAuthTokenModel>(authResponse);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(nameof(LightcastAccessTokenService), $"Deserialization failed. {ex.Message}");
                    return null;
                }
                return token;
            }
            else
            {
                _logger.LogInformation(nameof(LightcastAccessTokenService), $"Failed to receive information from auth/emsicloud");
                return null;
            }
        }
    }
}
