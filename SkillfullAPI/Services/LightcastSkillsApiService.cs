using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkillfullAPI.Models.LightcastApiModels;
using SkillfullAPI.Services.Interfaces;
using System.Net.Http.Headers;

namespace SkillfullAPI.Services
{
    public class LightcastSkillsApiService : ILightcastSkillsApiService
    {
        private const string LightcastTokenCacheKey = "LightcastToken";
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly HttpClient _apiClient;
        private readonly HttpClient _authClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LightcastSkillsApiService> _logger;
        private IMemoryCache _memoryCache;

        public LightcastSkillsApiService(HttpClient apiClient, HttpClient authClient, IConfiguration configuration, ILogger<LightcastSkillsApiService> logger, IMemoryCache memoryCache)
        {
            _apiClient = apiClient;
            _authClient = authClient;
            _configuration = configuration;
            _logger = logger;
            _memoryCache = memoryCache;
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
                    _logger.LogError($"Requested content was not returned");
                    return default(T);
                }
               
                T lightcastSkillsData = await DeserializeApiResponseAsync<T>(responseAsString);
                if (lightcastSkillsData == null)
                {
                    _logger.LogError("Deserialization was not successfull");
                    return default(T);
                }
                return lightcastSkillsData;
            }
            else
            {
                _logger.LogInformation($"Request to auth.emsicloud.com/connect/token returned {response.StatusCode}");
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
                    _logger.LogError($"Deserialization failed. {ex.Message}");
                    return default(T);
                }
                return result;
            }
            else
            {
                _logger.LogError($"Failed to receive information from Lightcast Api");
                return default(T);
            }
        }

        private async Task<LightcastAuthTokenModel> GetLightcastTokenAsync()
        {
            LightcastAuthTokenModel token = new();
            try
            {
                await semaphore.WaitAsync();
                if (_memoryCache.TryGetValue(LightcastTokenCacheKey, out LightcastAuthTokenModel value))
                {
                    token = value;
                    _logger.LogInformation($"Token found in cache.");
                }
                else
                {
                    _logger.LogInformation($"Token was not found in cache. Requesting new token from emsi.");
                    token = await RequestLightcastToken();
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(1800))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3540))
                        .SetPriority(CacheItemPriority.Normal)
                        .SetSize(1024);
                    _memoryCache.Set(LightcastTokenCacheKey, token, cacheEntryOptions);
                }
            }
            finally
            {
                semaphore.Release();
            }
            return token;
        }

        private async Task<LightcastAuthTokenModel> RequestLightcastToken()
        {
            var accessInfo = _configuration.GetSection("LightcastApi");
            if (accessInfo == null)
            {
                _logger.LogError($"Couldn't retrieve access information from secrets.json");
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
                    _logger.LogError("Requested content was not returned");
                    return null;
                }
                var token = await DeserializeApiResponseAsync<LightcastAuthTokenModel>(responseAsString);
                return token;
            }
            _logger.LogError($"Request was not successfull. Response: {response.StatusCode}");
            return null;
        }         
    }
}
