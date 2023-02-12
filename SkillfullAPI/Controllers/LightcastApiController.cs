using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using SkillfullAPI.Models;
using SkillfullAPI.Services.Interfaces;

namespace SkillfullAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LightcastApiController : ControllerBase
    {
        private readonly ILogger<LightcastApiController> _logger;
        private readonly ILightcastSkillsApiService _skillsApiService;

        public LightcastApiController(ILogger<LightcastApiController> logger, ILightcastSkillsApiService skillsApiService)
        {
            _logger = logger;
            _skillsApiService = skillsApiService;
        }


        [HttpGet]
        [Route("getAllSkills")]
        public async Task<SkillModelData> GetAllSkills()
        {
            SkillModelData result = await _skillsApiService.GetAllSkillsAsync();
            if(result == null)
            {
                return null;
            }
            return result;
        }
    }
}