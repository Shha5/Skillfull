using Newtonsoft.Json;
using SkillfullWebUI.Models.AuthModels;
using SkillfullWebUI.Models.SkillModels;
using SkillfullWebUI.Models.UserSkillsModels;
using SkillfullWebUI.Services.Interfaces;
using System.Web;


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
            SkillDetailsDataModel skillDetails = await DeserializeApiResponseAsync<SkillDetailsDataModel>(apiResponseString);
            
            return skillDetails.Data;
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

        //Change Password
        public async Task<HttpResponseMessage> ChangePassword(ChangePasswordModel changePassword, string userId)
        {
            string url = "https://localhost:7071/api/Auth/ChangePassword";
            var values = new Dictionary<string, string>()
            { { "UserId", userId },
              { "CurrentPassword", changePassword.Password },
              {"NewPassword", changePassword.NewPassword } };
            var requestContent = new FormUrlEncodedContent(values);
            return await _apiClient.PostAsync(url,requestContent);

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

        //USERSKILLS
        //Add userSkill
        public async Task<HttpResponseMessage> AddUserSkill(string userId, string skillId, string skillName, string skillAssessmentId)
        {
            string url = "https://localhost:7071/api/UserSkills/addUserSkill";
            var values = new Dictionary<string, string>()
            {
                {"UserId", userId },
                {"SkillId", skillId },
                {"SkillName", skillName},
                {"SkillAssessmentId", skillAssessmentId}
            };

            var requestContent = new FormUrlEncodedContent(values);

            return await _apiClient.PostAsync(url, requestContent);
        }

        //Get all userskills

        public async Task<List<UserSkillModel>> GetAllUserSkills(string userId)
        {
            string url = "https://localhost:7071/api/UserSkills/getAllUserSkills";
            string requestUri = string.Concat(url, "?userId=", userId);
            var apiResponse = await _apiClient.GetAsync(requestUri);
            if (apiResponse.IsSuccessStatusCode)
            {
                string apiResponseAsString = await apiResponse.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(apiResponseAsString))
                {
                    return null;
                }
                return await DeserializeApiResponseAsync<List<UserSkillModel>>(apiResponseAsString);
            }
            return null;
        }

        //update userskill

        //Delete userskill


        //Add task 

        //Get all user tasks

        //udate task

        //delete task



    }
}
