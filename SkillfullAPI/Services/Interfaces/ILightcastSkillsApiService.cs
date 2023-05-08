using SkillfullAPI.Models.LightcastApiModels;

namespace SkillfullAPI.Services.Interfaces
{
    public interface ILightcastSkillsApiService
    {
        Task<SkillModelData> GetAllSkillsAsync();
        Task<SkillDetailsModelData> GetSkillDetailsByIdAsync(string Id);
    }
}