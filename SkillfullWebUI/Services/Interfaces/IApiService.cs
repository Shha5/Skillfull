using SkillfullWebUI.Models.AuthModels;
using SkillfullWebUI.Models.SkillModels;

namespace SkillfullWebUI.Services.Interfaces
{
    public interface IApiService
    {
        Task<List<SkillModel>> GetAllSkills();
        Task<HttpResponseMessage> Register(RegistrationRequestModel registrationRequest);
        Task<HttpResponseMessage> ConfirmEmail(EmailConfirmationModel emailConfirmation);
        Task<HttpResponseMessage> ResendEmailConfirmation(ResendEmailConfirmationModel resendEmailConfirmation);
        Task<HttpResponseMessage> Login(LoginModel login);
    }
}
