using SkillfullWebUI.Models;
using SkillfullWebUI.Models.AuthModels;
using SkillfullWebUI.Models.SkillModels;
using SkillfullWebUI.Models.UserSkillsModels;

namespace SkillfullWebUI.Services.Interfaces
{
    public interface IApiService
    {
        //SKILLS
        Task<ApiServiceGetResponseModel<List<SkillModel>>> GetAllSkills();
        Task<ApiServiceGetResponseModel<SkillDetailsModel>> GetSkillDetailsById(string skillId);

        //AUTH
        Task<ApiServicePostResponseModel> ConfirmEmail(EmailConfirmationModel emailConfirmation);
        Task<ApiServicePostResponseModel> Login(LoginModel login);
        Task<ApiServicePostResponseModel> Register(RegistrationRequestModel registrationRequest);
        Task<ApiServicePostResponseModel> ResendEmailConfirmation(ResendEmailConfirmationModel resendEmailConfirmation);
        Task<ApiServicePostResponseModel> ForgotPassword(string email);
        Task<ApiServicePostResponseModel> ResetPassword(ResetPasswordModel resetPassword);
        Task<ApiServicePostResponseModel> ChangePassword(ChangePasswordModel changePassword);

        //USERSKILLS
        Task<ApiServicePostResponseModel> AddUserSkill(AddUserSkillViewModel addUserSkill);
        Task<ApiServiceGetResponseModel<List<UserSkillModel>>> GetAllUserSkills();
        Task<ApiServicePostResponseModel> UpdateUserSkill(string userSkillId, string newSkillAssessmentId);
        Task<ApiServicePostResponseModel> DeleteUserSkill(string userSkillId);
        Task<ApiServicePostResponseModel> AddTask(AddUserSkillTaskModel addUserSkillTask);
        Task<ApiServiceGetResponseModel<List<UserSkillTaskModel>>> GetAllTasksByUserId();
        Task<ApiServiceGetResponseModel<List<UserSkillTaskModel>>> GetAllTasksByUserSkillId(string userSkillId);
        Task<ApiServicePostResponseModel> ModifyTask(UpdateUserSkillTaskModel updateUserSkillTask);
        Task<ApiServicePostResponseModel> DeleteTask(string userSkillTaskId);
    }
}
