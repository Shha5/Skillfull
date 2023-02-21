using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using SkillfullAPI.Models;
using SkillfullAPI.Services.Interfaces;

namespace SkillfullAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly ILogger<SkillsController> _logger;
        private readonly ILightcastSkillsApiService _skillsApiService;

        public SkillsController(ILogger<SkillsController> logger, ILightcastSkillsApiService skillsApiService)
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

        [HttpGet]
        [Route("getSkillDetailsById")]
        public async Task<SkillDetailsModelData> GetSkillDetailsById(string Id)
        {
            SkillDetailsModelData result = await _skillsApiService.GetSkillDetailsByIdAsync(Id);
            if(result == null)
            {
                return null;
            }
            return result;
        }
    }
}