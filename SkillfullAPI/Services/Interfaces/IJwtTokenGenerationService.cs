using Microsoft.AspNetCore.Identity;

namespace SkillfullAPI.Services.Interfaces
{
    public interface IJwtTokenGenerationService
    {
        string GenerateJwtToken(IdentityUser user);
    }
}