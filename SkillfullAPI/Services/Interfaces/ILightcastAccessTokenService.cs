using SkillfullAPI.Models.LightcastApiModels;

namespace SkillfullAPI.Services.Interfaces
{
    public interface ILightcastAccessTokenService
    {
        Task<LightcastAuthTokenModel> GetLightcastTokenAsync();
    }
}