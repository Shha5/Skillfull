using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using SkillfullWebUI.Models.AuthModels;
using SkillfullWebUI.Models.SkillModels;
using SkillfullWebUI.Services.Interfaces;
using System.Web;
using static System.Net.WebRequestMethods;

namespace SkillfullWebUI.Services
{
    public class ApiService : IApiService
    {
        private readonly ILogger<ApiService> _logger;
        private readonly HttpClient _apiClient;


        public ApiService(ILogger<ApiService> logger, HttpClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;

        }
        //LIGHTCAST
        //get all skills 

        public async Task<List<SkillModel>> GetAllSkills()
        {
            string apiResponseString = await GetAllSkillsApiResponse();
            if (string.IsNullOrEmpty(apiResponseString))
            {
                return null;
            }
            SkillDataModel skillData = await DeserializeApiResponseAsync<SkillDataModel>(apiResponseString);
            List<SkillModel> skills = new List<SkillModel>();
            foreach (var skill in skillData.Data)
            {
                skills.Add(skill);
            }
            return skills;
        }

        private async Task<string> GetAllSkillsApiResponse()
        {
            string url = "https://localhost:7071/api/Skills/getAllSkills";
            var apiResponse = await _apiClient.GetAsync(url);
            if (apiResponse.IsSuccessStatusCode)
            {
                string responseAsString = await apiResponse.Content.ReadAsStringAsync();
                return responseAsString;
            }
            else
            {
                return null;
            }
        }

        //get skill details
        public async Task<SkillDetailsModel> GetSkillDetailsById(string skillId)
        {
            string apiResponseString = await GetSkillDetailsApiResponse(skillId);
            if (string.IsNullOrEmpty(apiResponseString))
            {
                return null;
            }
            SkillDetailsDataModel skillDetailsData = await DeserializeApiResponseAsync<SkillDetailsDataModel>(apiResponseString);
            SkillDetailsModel skillDetails = skillDetailsData.Data.FirstOrDefault();
            return skillDetails;
        }

        private async Task<string> GetSkillDetailsApiResponse(string skillId)
        {
            if (string.IsNullOrWhiteSpace(skillId))
            {
                return null;
            }
            string url = "https://localhost:7071/api/Skills/getSkillDetailsById?skillId=";
            var apiResponse = await _apiClient.GetAsync(string.Concat(url, skillId));
            if (apiResponse.IsSuccessStatusCode)
            {
                string responseAsString = await apiResponse.Content.ReadAsStringAsync();
                return responseAsString;
            }
            else
            {
                return null;
            }
        }

        //AUTH
        //Confirm email
        public async Task<HttpResponseMessage> ConfirmEmail(EmailConfirmationModel emailConfirmation)
        {
            string baseUrl = "https://localhost:7071/api/Auth/ConfirmEmail";
            var token = HttpUtility.UrlEncode(emailConfirmation.EmailConfirmationToken);
            string url = string.Concat(baseUrl, "?userId=", emailConfirmation.UserId, "&emailConfirmationToken=", token);
            return await _apiClient.PostAsync(url, null);

        }
        // Forgot password
        //Login
        public async Task<AuthResultModel> Login(LoginModel login)
        {
            string url = "https://localhost:7071/api/Auth/Login";
            var values = new Dictionary<string, string>()
            {
                { "Email", login.Email },
                { "Password", login.Password }
            };

            var requestContent = new FormUrlEncodedContent(values);
            var apiResponse = await _apiClient.PostAsync(url, requestContent);
            if (!apiResponse.IsSuccessStatusCode)
            {
                return new AuthResultModel()
                {
                    Errors = new List<string>()
                    {
                        "Something went wrong"
                    }
                };

            }
            else
            {
                string apiResponseAsString = await apiResponse.Content.ReadAsStringAsync();
                return await DeserializeApiResponseAsync<AuthResultModel>(apiResponseAsString);
            }

        }

        //Register
        public async Task<HttpResponseMessage> Register(RegistrationRequestModel registrationRequest)
        {
            string url = "https://localhost:7071/api/Auth/Register";
            var values = new Dictionary<string, string>()
            {
                {"Name", registrationRequest.Name },
                {"Email", registrationRequest.Email},
                {"Password", registrationRequest.Password},
                {"ConfirmPassword", registrationRequest.ConfirmPassword}
            };
            var requestContent = new FormUrlEncodedContent(values);

            return await _apiClient.PostAsync(url, requestContent);
        }
        //RefreshTOken --> not available to user directly
        public async Task<AuthResultModel> RefreshToken(string token, string refreshToken)
        {
            string url = "https://localhost:7071/api/Auth/RefreshToken";
            var values = new Dictionary<string, string>()
            { {"Token", token},
            {"RefreshToken", refreshToken} };
            var requestContent = new FormUrlEncodedContent(values);
            var apiResponse = await _apiClient.PostAsync(url, requestContent);
            if (apiResponse.IsSuccessStatusCode)
            {
                var apiResponseAsString = await apiResponse.Content.ReadAsStringAsync();
                return await DeserializeApiResponseAsync<AuthResultModel>(apiResponseAsString);
            }
            else
            {
                return null;
            }
        }

        //Resend Email confirmation
        public async Task<HttpResponseMessage> ResendEmailConfirmation(ResendEmailConfirmationModel resendEmailConfirmation)
        {
            string url = "https://localhost:7071/api/Auth/ResendEmailConfirmation";
            var values = new Dictionary<string, string>()
            {
                {"Email", resendEmailConfirmation.Email },
                { "Password", resendEmailConfirmation.Password}
            };

            var requestContent = new FormUrlEncodedContent(values);
            return await _apiClient.PostAsync(url, requestContent);
        }

        //Reset password
        public async Task<HttpResponseMessage> ForgotPassword(string email)
        {
            var values = new Dictionary<string, string>(){{ "Email", email }};
            var requestContent = new FormUrlEncodedContent(values);
            string url = "https://localhost:7071/api/Auth/ForgotPassword";
            return await _apiClient.PostAsync(url, requestContent);
        }

        public async Task<HttpResponseMessage> ResetPassword(ResetPasswordModel resetPassword)
        {
            string url = "https://localhost:7071/api/Auth/ResetPassword";
            var values = new Dictionary<string, string>()
            { { "passwordResetToken", resetPassword.PasswordResetToken },
              { "userId", resetPassword.UserId },
              { "newPassword", HttpUtility.UrlEncode(resetPassword.Password) }
            };
            var requestContent = new FormUrlEncodedContent(values);
            return await _apiClient.PostAsync(url, requestContent);
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
                    return default(T);
                }
                return result;
            }
            else
            {
                return default(T);
            }
        }

        

       
    }
}
