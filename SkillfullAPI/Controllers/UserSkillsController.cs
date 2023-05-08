using Microsoft.AspNetCore.Mvc;
using DataAccessLibrary.Data;
using SkillfullAPI.Models.AppModels;
using Microsoft.AspNetCore.Http.HttpResults;
using DataAccessLibrary.Data.Interfaces;

namespace SkillfullAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserSkillsController : ControllerBase
    {
       private readonly ILogger<SkillsController> _logger;
        private readonly IUserSkillsData _userSkillsData;
        
        public UserSkillsController(ILogger<SkillsController> logger, IUserSkillsData userSkillsData)
        {
            _logger = logger;
            _userSkillsData= userSkillsData;
        }

        //[HttpGet]
        //public async Task<List<UserSkillModel>> GetAllUserSkills(string userId)
        //{
        //    if(userId == null)
        //    {
        //        return null;
        //    }
        //    var result = _userSkillsData.GetUserSkills(userId).ToList();
        //    return result;
        //}

    }
}
