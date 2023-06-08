﻿using SkillfullWebUI.Models.AuthModels;
using SkillfullWebUI.Models.SkillModels;

namespace SkillfullWebUI.Services.Interfaces
{
    public interface IApiService
    {
        Task<List<SkillModel>> GetAllSkills();
        Task<SkillDetailsModel> GetSkillDetailsById(string skillId);
        Task<HttpResponseMessage> Register(RegistrationRequestModel registrationRequest);
        Task<HttpResponseMessage> ConfirmEmail(EmailConfirmationModel emailConfirmation);
        Task<HttpResponseMessage> ResendEmailConfirmation(ResendEmailConfirmationModel resendEmailConfirmation);
        Task<AuthResultModel> Login(LoginModel login);
        Task<HttpResponseMessage> ForgotPassword(string email);
        Task<HttpResponseMessage> ResetPassword(ResetPasswordModel resetPassword);
        Task<HttpResponseMessage> ChangePassword(ChangePasswordModel changePassword, string userId);
        Task<HttpResponseMessage> AddUserSkill(string userId, string skillId, string skillName, string skillAssessmentId);
    }
}
