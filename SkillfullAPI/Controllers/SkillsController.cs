using Microsoft.AspNetCore.Mvc;
using SkillfullAPI.Models.LightcastApiModels;
using SkillfullAPI.Services.Interfaces;

namespace SkillfullAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly ILightcastSkillsApiService _skillsApiService;

        public SkillsController(ILightcastSkillsApiService skillsApiService)
        {
            _skillsApiService = skillsApiService;
        }

        [HttpGet]
        [Route("GetAllSkills")]
        public async Task<SkillDataModel> GetAllSkills()
        {
            SkillDataModel result = await _skillsApiService.GetLightcastSkillsData<SkillDataModel>();
            if(result == null)
            {
                return null;
            }
            return result;
        }

        [HttpGet]
        [Route("GetSkillDetailsById")]
        public async Task<SkillDetailsDataModel> GetSkillDetailsById(string skillId)
        {
            SkillDetailsDataModel result = await _skillsApiService.GetLightcastSkillsData<SkillDetailsDataModel>(skillId);
            if(result == null)
            {
                return null;
            }
            return result;
        }
    }
}