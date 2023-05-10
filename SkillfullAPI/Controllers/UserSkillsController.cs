using Microsoft.AspNetCore.Mvc;
using DataAccessLibrary.Data;
using SkillfullAPI.Models.AppModels;
using Microsoft.AspNetCore.Http.HttpResults;
using DataAccessLibrary.Data.Interfaces;
using DataAccessLibrary.Models;
using System.Globalization;

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

        [HttpPost]
        [Route("addUserSkill")]
        public async void AddUserSkill(UserSkillModel userSkillModel, string userId)
        {
            if (ModelState.IsValid)
            {
                UserSkillDataModel userSkill = new UserSkillDataModel()
                {
                    SkillId = userSkillModel.SkillId,
                    SkillName = userSkillModel.SkillName,
                    Id = null,
                    SkillAssessmentId = userSkillModel.SkillAssessmentId
                };

               await _userSkillsData.AddUserSkill(userId, userSkill);
            } 
        }

        [HttpGet]
        [Route("getAllUserSkills")]
        public async Task<List<UserSkillModel>> GetAllUserSkills(string userId)
        {
            if (userId == null)
            {
                return null;
            }
            var result = _userSkillsData.GetUserSkills(userId).ToList();
            List<UserSkillModel> userSkills = new List<UserSkillModel>();
            if(result.Count == 0)
            {
                return null;
            }
            foreach(var item in result)
            {
                userSkills.Add(new UserSkillModel()
                {
                    SkillId = item.SkillId,
                    SkillName = item.SkillName,
                    SkillAssessmentId = item.SkillAssessmentId
                });
            }
            return userSkills;
        }
    }
}
