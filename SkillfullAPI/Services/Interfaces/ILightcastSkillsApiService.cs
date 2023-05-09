using SkillfullAPI.Models.LightcastApiModels;

namespace SkillfullAPI.Services.Interfaces
{
    public interface ILightcastSkillsApiService
    {
        Task<T> GetLightcastSkillsData<T>(string? skillId = null);
    }
}