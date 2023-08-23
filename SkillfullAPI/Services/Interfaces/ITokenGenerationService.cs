using Microsoft.AspNetCore.Identity;
using SkillfullAPI.Models.AuthModels;
using SkillfullAPI.Models.AuthModels.DTOs;

namespace SkillfullAPI.Services.Interfaces
{
    public interface ITokenGenerationService
    {
        Task<AuthResultModel> GenerateJwtToken(IdentityUser user);
        Task<AuthResultModel> VerifyAndGenerateToken(TokenRequestDto tokenRequest);
        Task<bool> IsTokenValid(string token); 
    }
}