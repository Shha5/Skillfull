using SkillfullAPI.Models;

namespace SkillfullAPI.Services.Interfaces
{
    public interface ILightcastSkillsApiService
    {
        Task<SkillModelData> GetAllSkillsAsync();
        Task<SkillDetailsModel> GetSkillDetailsByIdAsync(string Id);
    }
}