using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SkillfullAPI.Models.LightcastApiModels;
using SkillfullAPI.Services.Interfaces;
using System.Net.Http.Headers;

namespace SkillfullAPI.Services
{
    public class LightcastSkillsApiService : ILightcastSkillsApiService
    {
        private readonly HttpClient _apiClient;
        private readonly ILightcastAccessTokenService _lightcastAccessTokenService;
        private readonly ILogger<LightcastSkillsApiService> _logger;

        public LightcastSkillsApiService(HttpClient apiClient, ILightcastAccessTokenService lightcastAccessTokenService, ILogger<LightcastSkillsApiService> logger)
        {
            _apiClient = apiClient;
            _lightcastAccessTokenService = lightcastAccessTokenService;
            _logger = logger;
        }

        public async Task<SkillModelData> GetAllSkillsAsync()
        {
            string response = await GetAllSkillsResponseAsync();
            if (!string.IsNullOrEmpty(response))
            {
                return await DeserializeSkillsApiResponseAsync(response);
            }
            else
            {
                return null;
            }
        }

        public async Task<SkillDetailsModelData> GetSkillDetailsByIdAsync(string Id)
        {
            string response = await GetSkillDetailsResponseByIdAsync(Id);
            if (!string.IsNullOrEmpty(response))
            {
                return await DeserializeSkillDetailsResponse(response);
            }
            else
            {
                return null;
            }
        }


        //THINK OF HOW TO MAKE IT MORE DRY
        private async Task<string> GetAllSkillsResponseAsync()
        {
            var token = await _lightcastAccessTokenService.GetLightcastTokenAsync();
            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.token_type, token.access_token);
            var response = await _apiClient.GetAsync("https://emsiservices.com/skills/versions/latest/skills");

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

        private async Task<string> GetSkillDetailsResponseByIdAsync(string skillId)
        {
            var token = await _lightcastAccessTokenService.GetLightcastTokenAsync();
            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token.token_type, token.access_token);
            var response = await _apiClient.GetAsync(string.Concat("https://emsiservices.com/skills/versions/latest/skills/", skillId));

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

        private async Task<SkillDetailsModelData> DeserializeSkillDetailsResponse(string skillDetailsResponse)
        {
            if (!string.IsNullOrEmpty(skillDetailsResponse))
            {
                SkillDetailsModelData result;

                try
                {
                    result = JsonConvert.DeserializeObject<SkillDetailsModelData>(skillDetailsResponse);

                }
                catch (Exception ex)
                {
                    _logger.LogInformation(nameof(LightcastAccessTokenService), $"Deserialization failed. {ex.Message}");
                    return null;
                }
                return result;
            }
            else
            {
                _logger.LogInformation(nameof(LightcastAccessTokenService), $"Failed to receive information from auth/emsicloud");
                return null;
            }
        }

        private async Task<SkillModelData> DeserializeSkillsApiResponseAsync(string skillsResponse)
        {
            if (!string.IsNullOrEmpty(skillsResponse))
            {
               SkillModelData result;

                try
                {
                    result = JsonConvert.DeserializeObject<SkillModelData>(skillsResponse);

                }
                catch (Exception ex)
                {
                    _logger.LogInformation(nameof(LightcastAccessTokenService), $"Deserialization failed. {ex.Message}");
                    return null;
                }
                return result;
            }
            else
            {
                _logger.LogInformation(nameof(LightcastAccessTokenService), $"Failed to receive information from auth/emsicloud");
                return null;
            }
        }

    }
}
