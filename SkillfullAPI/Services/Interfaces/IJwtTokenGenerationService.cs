using Microsoft.AspNetCore.Identity;
using SkillfullAPI.Models.AuthorizationModels;

namespace SkillfullAPI.Services.Interfaces
{
    public interface IJwtTokenGenerationService
    {
        Task<AuthResultModel> GenerateJwtToken(IdentityUser user);
        Task<AuthResultModel> VerifyAndGenerateToken(TokenRequestDto tokenRequest);
    }
}