using SkillfullAPI.Models;

namespace SkillfullAPI.Services.Interfaces
{
    public interface ILightcastAccessTokenService
    {
        Task<LightcastAuthTokenModel> GetLightcastTokenAsync();
    }
}