using Newtonsoft.Json;
using SkillfullWebUI.Constants;
using SkillfullWebUI.Models;
using SkillfullWebUI.Models.AuthModels;
using SkillfullWebUI.Models.SkillModels;
using SkillfullWebUI.Models.UserSkillsModels;
using SkillfullWebUI.Services.Interfaces;
using System.Net.Http.Headers;
using System.Web;


namespace SkillfullWebUI.Services
{
    public class ApiService : IApiService
    {
        private readonly ILogger<ApiService> _logger;
        private readonly HttpClient _apiClient;
        private readonly ICookieManagerService _cookieManager;


        public ApiService(ILogger<ApiService> logger, HttpClient apiClient, ICookieManagerService cookieManager)
        {
            _logger = logger;
            _apiClient = apiClient;
            _cookieManager = cookieManager;
        }
        // ALL SKILLS

        public async Task<ApiServiceGetResponseModel<List<SkillModel>>> GetAllSkills()
        {
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.GetAllSkills);
            var response = await _apiClient.GetAsync(url);
            if (response.IsSuccessStatusCode == false)
            {
                return new ApiServiceGetResponseModel<List<SkillModel>>()
                {
                    Result = false,
                    ErrorMessage = "Couldn't retrieve skills list"
                };
            }
            var responseString = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseString))
            {
                return new ApiServiceGetResponseModel<List<SkillModel>>()
                {
                    Result = false,
                    ErrorMessage = "Couldn't retrieve skills list"
                };
 
            }
            SkillDataModel skillData = await DeserializeApiResponseAsync<SkillDataModel>(responseString);
            List<SkillModel> skills = new List<SkillModel>();
            foreach (var skill in skillData.Data)
            {
                skills.Add(skill);
            }
            return new ApiServiceGetResponseModel<List<SkillModel>>()
            {
                Result = true,
                Content = skills
            };
        }

        public async Task<ApiServiceGetResponseModel<SkillDetailsModel>> GetSkillDetailsById(string skillId)
        {
            if (string.IsNullOrWhiteSpace(skillId))
            {
                return null;
            }
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.GetSkillDetailsById, "?skillId=", skillId);
            var response = await _apiClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(responseString))
                {
                    return new ApiServiceGetResponseModel<SkillDetailsModel>()
                    {
                        Result = false,
                        Content = null,
                        ErrorMessage = "Couldn't retrieve skill details."
                    };
                }
                SkillDetailsDataModel skillDetails = await DeserializeApiResponseAsync<SkillDetailsDataModel>(responseString);
                if (skillDetails.Data == null)
                {
                    return new ApiServiceGetResponseModel<SkillDetailsModel>()
                    {
                        Result = false,
                        Content = null,
                        ErrorMessage = "Couldn't retrieve skill details."
                    };
                }
                return new ApiServiceGetResponseModel<SkillDetailsModel>()
                {
                    Result = true,
                    Content = skillDetails.Data
                };
            }
            return new ApiServiceGetResponseModel<SkillDetailsModel>()
            {
                Result = false,
                Content = null,
                ErrorMessage = "Couldn't retrieve skill details."
            };
        }  

        //AUTH
        
        public async Task<ApiServicePostResponseModel> ConfirmEmail(EmailConfirmationModel emailConfirmation)
        {
            var token = HttpUtility.UrlEncode(emailConfirmation.EmailConfirmationToken);
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.ConfirmEmail, "?userId=", emailConfirmation.UserId, "&emailConfirmationToken=", token);
            var response = await _apiClient.PostAsync(url, null);
            if (response.IsSuccessStatusCode == false)
            {
                return new ApiServicePostResponseModel()
                {
                    Result = false,
                    ErrorMessage = response.StatusCode.ToString()
                };
            }
            return new ApiServicePostResponseModel()
            {
                Result = true,
                ErrorMessage = null
            };
        }
       
        public async Task<ApiServicePostResponseModel> Login(LoginModel login)
        {
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.Login);
            var values = new Dictionary<string, string>()
            {
                { "Email", login.Email },
                { "Password", login.Password }
            };

            var requestContent = new FormUrlEncodedContent(values);
            var apiResponse = await _apiClient.PostAsync(url, requestContent);
            if (apiResponse.IsSuccessStatusCode == false)
            {
                return new ApiServicePostResponseModel()
                {
                    Result = false,
                    ErrorMessage = "Something went wrong"
                };
            }
            else
            {
                string apiResponseAsString = await apiResponse.Content.ReadAsStringAsync();
                var authResult = await DeserializeApiResponseAsync<AuthResultModel>(apiResponseAsString);
                if(authResult == null || authResult.Result == false)
                {
                    return new ApiServicePostResponseModel()
                    {
                        Result = false,
                        ErrorMessage = "Something went wrong"
                    };
                }
                _cookieManager.CreateAuthCookies(authResult, login.RememberMe);
                return new ApiServicePostResponseModel() 
                { Result = true };
            }
        }

        public async Task<ApiServicePostResponseModel> Register(RegistrationRequestModel registrationRequest)
        {
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.Register);
            var values = new Dictionary<string, string>()
            {
                {"Name", registrationRequest.Name },
                {"Email", registrationRequest.Email},
                {"Password", registrationRequest.Password},
                {"ConfirmPassword", registrationRequest.ConfirmPassword}
            };
            var requestContent = new FormUrlEncodedContent(values);

            var response = await _apiClient.PostAsync(url, requestContent);
            if(response.IsSuccessStatusCode == false)
            {
                return new ApiServicePostResponseModel()
                {
                    Result = false,
                    ErrorMessage = response.StatusCode.ToString()
                };
            }
            return new ApiServicePostResponseModel()
            {
                Result = true,
                ErrorMessage = null
            };
        }

        public async Task<ApiServicePostResponseModel> ResendEmailConfirmation(ResendEmailConfirmationModel resendEmailConfirmation)
        {
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.ResendEmailConfirmation);
            var values = new Dictionary<string, string>()
            {
                {"Email", resendEmailConfirmation.Email },
                { "Password", resendEmailConfirmation.Password}
            };

            var requestContent = new FormUrlEncodedContent(values);
            var response = await _apiClient.PostAsync(url, requestContent);
            if (response.IsSuccessStatusCode == false)
            {
                return new ApiServicePostResponseModel()
                {
                    Result = false,
                    ErrorMessage = response.StatusCode.ToString()
                };
            }
            return new ApiServicePostResponseModel()
            {
                Result = true,
                ErrorMessage = null
            };
        }

        public async Task<ApiServicePostResponseModel> ForgotPassword(string email)
        {
            var values = new Dictionary<string, string>(){{ "Email", email }};
            var requestContent = new FormUrlEncodedContent(values);
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.ForgotPassword);
            var response = await _apiClient.PostAsync(url, requestContent);
            if(response.IsSuccessStatusCode == false)
            {
                return new ApiServicePostResponseModel()
                {
                    Result = false,
                    ErrorMessage = response.StatusCode.ToString()
                };
            }
            return new ApiServicePostResponseModel()
            {
                Result = true,
                ErrorMessage = null
            };
        }

        public async Task<ApiServicePostResponseModel> ResetPassword(ResetPasswordModel resetPassword)
        {
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.ResetPassword);
            var values = new Dictionary<string, string>()
            { { "passwordResetToken", resetPassword.PasswordResetToken },
              { "userId", resetPassword.UserId },
              { "newPassword", HttpUtility.UrlEncode(resetPassword.Password) }
            };
            var requestContent = new FormUrlEncodedContent(values);
            var response = await _apiClient.PostAsync(url, requestContent);
            if(response.IsSuccessStatusCode == false)
            {
                return new ApiServicePostResponseModel()
                {
                    Result = false,
                    ErrorMessage = response.StatusCode.ToString()
                };
            }
            return new ApiServicePostResponseModel()
            {
                Result = true,
                ErrorMessage = null
            };
        }

        public async Task<ApiServicePostResponseModel> ChangePassword(ChangePasswordModel changePassword)
        {
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.ChangePassword);
            var authCookies = _cookieManager.GetAuthCookieValues();
            var values = new Dictionary<string, string>()
            { { "UserId", authCookies.UserId },
              { "CurrentPassword", changePassword.Password },
              {"NewPassword", changePassword.NewPassword } };
            var requestContent = new FormUrlEncodedContent(values);
            var response = await _apiClient.PostAsync(url,requestContent);
            if(response.IsSuccessStatusCode == false)
            {
                return new ApiServicePostResponseModel()
                {
                    Result = false,
                    ErrorMessage = response.StatusCode.ToString()
                };
            }
            return new ApiServicePostResponseModel()
            {
                Result = true,
                ErrorMessage = null
            };
        }

        //USERSKILLS
        public async Task<ApiServicePostResponseModel> AddUserSkill(AddUserSkillViewModel addUserSkill)
        {
            var cookieVerification = await VerifyAndRefreshCookies();
            if (cookieVerification.Result == false) 
            {
                return cookieVerification;
            }
            var authCookies = _cookieManager.GetAuthCookieValues();
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.AddUserSkill);
            var values = new Dictionary<string, string>()
            {
                {"UserId", authCookies.UserId },
                {"SkillId", addUserSkill.SkillId },
                {"SkillName", addUserSkill.SkillName},
                {"SkillAssessmentId", addUserSkill.SkillAssessmentId}
            };

            var requestContent = new FormUrlEncodedContent(values);

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authCookies.Token);
            var result = await _apiClient.PostAsync(url, requestContent);
            if (result.IsSuccessStatusCode)
            {
                return new ApiServicePostResponseModel()
                {
                    Result = true,
                    ErrorMessage = string.Empty
                };
            }
            else
            {
                return new ApiServicePostResponseModel()
                {
                    Result = false,
                    ErrorMessage = result.StatusCode.ToString()
                };
            }

        }

        public async Task<ApiServiceGetResponseModel<List<UserSkillModel>>> GetAllUserSkills()
        {
            var cookieVerification = await VerifyAndRefreshCookies();
            if (cookieVerification.Result == false)
            {
                return new ApiServiceGetResponseModel<List<UserSkillModel>>()
                {
                    Result = false,
                    ErrorMessage = cookieVerification.ErrorMessage,
                    Content = null
                };
            }
            var authCookies =  _cookieManager.GetAuthCookieValues();
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.GetAllUserSkills, "?userId=", authCookies.UserId);
        
            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authCookies.Token);
           
            var response = await _apiClient.GetAsync(url);
           
            if(response.IsSuccessStatusCode && response.Content == null)
            {
              return new ApiServiceGetResponseModel<List<UserSkillModel>>()
               {
                    Result = true,
                    ErrorMessage = "No userskills were added",
                    Content = null
               };
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            var result = await DeserializeApiResponseAsync<List<UserSkillModel>>(responseContent);
            return new ApiServiceGetResponseModel<List<UserSkillModel>>()
            {
                Result = true,
                Content = result,
                ErrorMessage = null
            };
        }

        public async Task<ApiServicePostResponseModel> UpdateUserSkill(string userSkillId, string newSkillAssessmentId)
        {
            var cookieVerification = await VerifyAndRefreshCookies();
            if (cookieVerification.Result == false)
            {
                return cookieVerification;
            }
            var authCookies = _cookieManager.GetAuthCookieValues();
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.UpdateUserSkill);
            var values = new Dictionary<string, string>()
            {
                {"userSkillId", userSkillId },
                {"newSkillAssessmentId", newSkillAssessmentId}
            };

            var requestContent = new FormUrlEncodedContent(values);
            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authCookies.Token);

            var result = await _apiClient.PostAsync(url, requestContent);
            if (result.IsSuccessStatusCode == false)
            {
                return new ApiServicePostResponseModel()
                {
                    Result = false,
                    ErrorMessage = result.StatusCode.ToString()
                };
            }
            return new ApiServicePostResponseModel()
            {
                Result = true,
                ErrorMessage = null,
            };
        }

        public async Task<ApiServicePostResponseModel> DeleteUserSkill(string userSkillId)
        {
            var cookieVerification = await VerifyAndRefreshCookies();
            if (cookieVerification.Result == false)
            {
                return cookieVerification;
            }
            var authCookies = _cookieManager.GetAuthCookieValues();
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.DeleteUserSkill);
            var values = new Dictionary<string, string>()
            {
                {"userSkillId", userSkillId }
            };

            var requestContent = new FormUrlEncodedContent(values);
            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authCookies.Token);
            var response = await _apiClient.PostAsync(url, requestContent);
            if(response.IsSuccessStatusCode == false)
            {
                return new ApiServicePostResponseModel()
                {
                    Result = false,
                    ErrorMessage = response.StatusCode.ToString()
                };
            }
            return new ApiServicePostResponseModel()
            {
                Result = true,
                ErrorMessage = null
            };
        }

        public async Task<ApiServicePostResponseModel> AddTask(AddUserSkillTaskModel addUserSkillTask)
        {
            var cookieVerification = await VerifyAndRefreshCookies();
            if (cookieVerification.Result == false)
            {
                return cookieVerification;
            }
            var authCookies = _cookieManager.GetAuthCookieValues();
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.AddTask);
            var values = new Dictionary<string, string>()
            {
                
                {"userId", authCookies.UserId},
                {"TaskName", addUserSkillTask.TaskName },
                { "TaskDescription", addUserSkillTask.TaskDescription },
                { "TaskStatusId", addUserSkillTask.TaskStatusId},
                {"UserSkillId", addUserSkillTask.UserSkillId },
                {"UserSkillName", addUserSkillTask.UserSkillName }
            };

            var requestContent = new FormUrlEncodedContent(values);
            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authCookies.Token);
            var response = await _apiClient.PostAsync(url,requestContent);
            if(response.IsSuccessStatusCode == false)
            {
                return new ApiServicePostResponseModel() 
                {
                    Result = false,
                    ErrorMessage = response.StatusCode.ToString()
                };
            }
            return new ApiServicePostResponseModel()
            {
                Result = true,
                ErrorMessage = null
            };
        }


        public async Task<ApiServiceGetResponseModel<List<UserSkillTaskModel>>> GetAllTasksByUserId()
        {
            var cookieVerification = await VerifyAndRefreshCookies();
            if (cookieVerification.Result == false)
            {
                return new ApiServiceGetResponseModel<List<UserSkillTaskModel>>()
                {
                    Result = false,
                    ErrorMessage = cookieVerification.ErrorMessage,
                    Content = null
                };
            }
            var authCookies = _cookieManager.GetAuthCookieValues();
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.GetAllTasksByUserId, "?userId=", authCookies.UserId);
            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authCookies.Token);
           var response = await _apiClient.GetAsync(url);
            if(response.IsSuccessStatusCode == false)
            {
                return new ApiServiceGetResponseModel<List<UserSkillTaskModel>>() 
                {
                    Result = false,
                    ErrorMessage = response.StatusCode.ToString()
                };
            }
            if (response.IsSuccessStatusCode && response.Content == null)
            {
                return new ApiServiceGetResponseModel<List<UserSkillTaskModel>>()
                {
                    Result = true,
                    ErrorMessage = "No content",
                    Content = null
                };
            }
            string responseString = await response.Content.ReadAsStringAsync();
            var result = await DeserializeApiResponseAsync<List<UserSkillTaskModel>>(responseString);
            return new ApiServiceGetResponseModel<List<UserSkillTaskModel>>() 
            {
                Result = true,
                ErrorMessage = null,
                Content = result
            };
        }

        public async Task<ApiServiceGetResponseModel<List<UserSkillTaskModel>>> GetAllTasksByUserSkillId(string userSkillId)
        {
            var cookieVerification = await VerifyAndRefreshCookies();
            if (cookieVerification.Result == false)
            {
                return new ApiServiceGetResponseModel<List<UserSkillTaskModel>>()
                {
                    Result = false,
                    ErrorMessage = cookieVerification.ErrorMessage,
                    Content = null
                };
            }
            var authCookies = _cookieManager.GetAuthCookieValues();
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.GetAllTasksByUserSkillId, "?userSkillId=", userSkillId);

            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authCookies.Token);
            var response = await _apiClient.GetAsync(url); 
            if(response.IsSuccessStatusCode == false)
            {
                return new ApiServiceGetResponseModel<List<UserSkillTaskModel>>()
                {
                    Result = false,
                    ErrorMessage = response.StatusCode.ToString(),
                    Content = null
                };
            }
            if(response.IsSuccessStatusCode && response.Content == null)
            {
                return new ApiServiceGetResponseModel<List<UserSkillTaskModel>>()
                {
                    Result = true,
                    ErrorMessage = "No content",
                    Content = null
                };
            }
            string responseString = await response.Content.ReadAsStringAsync();
            var result = await DeserializeApiResponseAsync<List<UserSkillTaskModel>>(responseString);
            return new ApiServiceGetResponseModel<List<UserSkillTaskModel>>()
            {
                Result = true,
                ErrorMessage = null,
                Content = result
            };
        }

        public async Task<ApiServicePostResponseModel> ModifyTask(UpdateUserSkillTaskModel updateUserSkillTask)
        {
            var cookieVerification = await VerifyAndRefreshCookies();
            if (cookieVerification.Result == false)
            {
                return cookieVerification;
            }
            var authCookies = _cookieManager.GetAuthCookieValues();
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.ModifyTask);
            var values = new Dictionary<string, string>()
            {
                {"UserSkillTaskId", updateUserSkillTask.UserSkillTaskId },
                {"NewTaskName", updateUserSkillTask.NewTaskName },
                {"NewTaskDescription", updateUserSkillTask.NewTaskDescription},
                {"NewTaskStatusId", updateUserSkillTask.NewTaskStatusId}
            };
            var requestContent = new FormUrlEncodedContent(values);
            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authCookies.Token);            
            var response = await _apiClient.PostAsync(url, requestContent);
            if(response.IsSuccessStatusCode == false)
            {
                return new ApiServicePostResponseModel()
                {
                    Result = false,
                    ErrorMessage = response.StatusCode.ToString()
                };
            }
            return new ApiServicePostResponseModel()
            {
                Result = true,
                ErrorMessage = null
            };

        }

        public async Task<ApiServicePostResponseModel> DeleteTask( string userSkillTaskId)
        {
            var cookieVerification = await VerifyAndRefreshCookies();
            if (cookieVerification.Result == false)
            {
                return cookieVerification;
            }
            var authCookies = _cookieManager.GetAuthCookieValues();
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.DeleteTask);
            var values = new Dictionary<string, string>()
            {
                { "userSkillTaskId", userSkillTaskId }
            };
            var requestContent = new FormUrlEncodedContent(values);
            _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authCookies.Token);
            var response = await _apiClient.PostAsync(url, requestContent);
            if(response.IsSuccessStatusCode == false)
            {
                return new ApiServicePostResponseModel()
                {
                    Result = false,
                    ErrorMessage = response.StatusCode.ToString()
                };
            }
            return new ApiServicePostResponseModel()
            {
                Result = true,
                ErrorMessage = null
            };
        }

        //PRIVATE METHODS
        private async Task<HttpResponseMessage> CheckIfTokenIsValid(string token)
        {
            var values = new Dictionary<string, string>()
            {
                { "token", token }
            };
            string url = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.CheckIfTokenIsValid);
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

        private async Task<AuthResultModel> RefreshToken(string token, string refreshToken)
        {
            string uri = string.Concat(SkillfullApiEndpoints.BaseUrl, SkillfullApiEndpoints.RefreshToken);
            var values = new Dictionary<string, string>()
            { {"Token", token},
            {"RefreshToken", refreshToken} };
            var requestContent = new FormUrlEncodedContent(values);
            var apiResponse = await _apiClient.PostAsync(uri, requestContent);
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

        private async Task<ApiServicePostResponseModel> VerifyAndRefreshCookies()
        {
            if (_cookieManager.AreAuthCookiesPresent() == false)
            {
                return new ApiServicePostResponseModel()
                {
                    Result = false,
                    ErrorMessage = "User is not logged in"
                };
            }
            var authCookies = _cookieManager.GetAuthCookieValues();
            var rememberMe = _cookieManager.IsRememberMeCookiePresent();

            var tokenValidation = await CheckIfTokenIsValid(authCookies.Token);
            var tokenValidationString = await tokenValidation.Content.ReadAsStringAsync();
            if (tokenValidationString == null)
            {
                _cookieManager.RemoveAuthCookies();
                return new ApiServicePostResponseModel()
                {
                    Result = false,
                    ErrorMessage = "An error occured. Logging in is required."
                };
            }
            if (tokenValidationString.Contains("false"))
            {
                var refreshedAuthResult = await RefreshToken(authCookies.Token, authCookies.RefreshToken);
                if (refreshedAuthResult == null || refreshedAuthResult.Result == false)
                {
                    _cookieManager.RemoveAuthCookies();
                    return new ApiServicePostResponseModel()
                    {
                        Result = false,
                        ErrorMessage = "An error occured. Logging in is required."
                    };
                }
                _cookieManager.RemoveAuthCookies();
                _cookieManager.CreateAuthCookies(refreshedAuthResult, rememberMe);
                return new ApiServicePostResponseModel()
                {
                    Result = true,
                    ErrorMessage = string.Empty
                };
            }

            return new ApiServicePostResponseModel()
            {
                Result = true,
                ErrorMessage = string.Empty
            };
        }

        //private async Task<string> GetAllSkillsApiResponse()
        //{
        //    ;
        //    var apiResponse = await _apiClient.GetAsync(uri);
        //    if (apiResponse.IsSuccessStatusCode)
        //    {
        //        string responseAsString = await apiResponse.Content.ReadAsStringAsync();
        //        return responseAsString;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //private async Task<string> GetSkillDetailsApiResponse(string skillId)
        //{
        //    if (string.IsNullOrWhiteSpace(skillId))
        //    {
        //        return null;
        //    }
        //    string uri = string.Concat(SkillfullApiEndpoints.BaseUri,SkillfullApiEndpoints.GetSkillDetailsById, "?skillId=", skillId);

        //    if (apiResponse.IsSuccessStatusCode)
        //    {
        //        string responseAsString = await apiResponse.Content.ReadAsStringAsync();
        //        return responseAsString;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
    }
}
