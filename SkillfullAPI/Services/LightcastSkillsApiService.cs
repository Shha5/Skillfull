using Newtonsoft.Json;
using SkillfullAPI.Models.LightcastApiModels;
using SkillfullAPI.Services.Interfaces;
using System.Net.Http.Headers;

namespace SkillfullAPI.Services
{
    public class LightcastSkillsApiService : ILightcastSkillsApiService
    {
        private readonly HttpClient _apiClient;
        private readonly HttpClient _authClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LightcastSkillsApiService> _logger;

        public LightcastSkillsApiService(HttpClient apiClient, HttpClient authClient, IConfiguration configuration, ILogger<LightcastSkillsApiService> logger)
        {
            _apiClient = apiClient;
            _authClient = authClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<T> GetLightcastSkillsData<T>(string? skillId = null) 
        {
            var token = await GetLightcastTokenAsync();
            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.TokenType, token.AccessToken);
            
            string requestUri = CreateRequestUri(skillId);
            var response = await _apiClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                string responseAsString = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseAsString))
                {
                    return default(T);
                }
               
                T lightcastSkillsData = await DeserializeApiResponseAsync<T>(responseAsString);
                if(lightcastSkillsData == null)
                {
                    return default(T);
                }
                return lightcastSkillsData;
            }
            else
            {
                _logger.LogInformation(nameof(LightcastSkillsApiService), $"Request to auth.emsicloud.com/connect/token returned {response.StatusCode}");
                return default(T);
            }
        }

        private string CreateRequestUri(string? skillId = null)
        {
            string requestUri;

            if (!string.IsNullOrEmpty(skillId))
            {
                requestUri = string.Concat("https://emsiservices.com/skills/versions/latest/skills/", skillId);
            }
            else
            {
                requestUri = "https://emsiservices.com/skills/versions/latest/skills";
            }

            return requestUri;
        }

        private async Task<T> DeserializeApiResponseAsync<T>(string responseJson)
        {
            if (!string.IsNullOrEmpty(responseJson))
            {
                T result;
                try
                {
                    result = JsonConvert.DeserializeObject<T>(responseJson);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(nameof(LightcastSkillsApiService), $"Deserialization failed. {ex.Message}");
                    return default(T);
                }
                return result;
            }
            else
            {
                _logger.LogInformation(nameof(LightcastSkillsApiService), $"Failed to receive information from Lightcast Api");
                return default(T);
            }
        }

        private async Task<LightcastAuthTokenModel> GetLightcastTokenAsync()
        {
            var accessInfo = _configuration.GetSection("LightcastApi");

            if (accessInfo == null)
            {
                _logger.LogInformation(nameof(LightcastSkillsApiService), $"Couldn't retrieve access information from secrets.json");
                return null;
            }

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
                string responseAsString = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseAsString))
                {
                    return null;
                }

                LightcastAuthTokenModel token = await DeserializeApiResponseAsync<LightcastAuthTokenModel>(responseAsString);
                if (token == null)
                {
                    return null;
                }
                return token;
            }
            else
            {
                _logger.LogInformation(nameof(LightcastSkillsApiService), $"Request to auth.emsicloud.com/connect/token returned {response.StatusCode}");
                return null;
            }
        }
    }
}
