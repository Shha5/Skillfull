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
        public async Task<string> AddUserSkill(UserSkillModel userSkillModel, string userId)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(userId))
            {
                UserSkillDataModel userSkill = new UserSkillDataModel()
                {
                    SkillId = userSkillModel.SkillId,
                    SkillName = userSkillModel.SkillName,
                    Id = null,
                    SkillAssessmentId = userSkillModel.SkillAssessmentId
                };

               await _userSkillsData.AddUserSkill(userId, userSkill);
                return "Skill successfully assigned to a user.";
            } 
            else
            {
                return "Request not valild.";
            }
        }

        [HttpGet]
        [Route("getAllUserSkills")]
        public async Task<List<UserSkillModel>> GetAllUserSkills(string userId)
        {
            if (string.IsNullOrEmpty(userId))
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

        [HttpPost]
        [Route("updateUserSkillAssessment")]
        public async Task<IActionResult> UpdateUserSkillAssessment(int userSkillId, int newUserSkillAssessmentId)
        {
            if (newUserSkillAssessmentId == null || newUserSkillAssessmentId <= 0 || newUserSkillAssessmentId > 5 || userSkillId == null)
            {
                return BadRequest("Request not valid");
            }
            await _userSkillsData.UpdateUserSkillAssessment(userSkillId, newUserSkillAssessmentId);
            return Ok("Successfully updated.");
        }

        [HttpPost]
        [Route("deleteUserSkill")]
        public async Task<IActionResult> DeleteUserSkill(int userSkillId)
        {
            if(userSkillId == null)
            {
                return BadRequest("Invalid request");
            }
            await _userSkillsData.DeleteUserSkills(userSkillId);
            return Ok("Record deleted permanently.");
        }

        [HttpPost]
            [Route("addUserSkillTask")]
        public async Task<string> AddUserSkillTask(string userId, UserSkillTaskModel userSkillTaskModel)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(userId))
            {
                UserSkillTaskDataModel userSkillTask = new UserSkillTaskDataModel()
                {
                    Name = userSkillTaskModel.Name,
                    Description = userSkillTaskModel.Description,
                    StatusId = userSkillTaskModel.StatusId,
                    UserSkillId = userSkillTaskModel.UserSkillId,
                    UserId = userId
                };
                try
                {
                    await _userSkillsData.AddUserSkillTask(userId, userSkillTask);
                }
                catch(Exception ex)
                {
                    return ex.Message;
                }
                return "Task successfully added to user and skill.";
            }
            else
            {
                return "Request not valid";
            }
        }

        [HttpGet]
        [Route("getAllUserSkillTasksPerUser")]
        public async Task<List<UserSkillTaskModel>> GetAllUserSkillTasksPerUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }
            var result = _userSkillsData.GetUserSkillTasksPerUser(userId).ToList();
            List<UserSkillTaskModel> userSkillTasks = new List<UserSkillTaskModel>();
            if (result.Count == 0)
            {
                return null;
            }
            foreach (var item in result)
            {
                userSkillTasks.Add(new UserSkillTaskModel()
                {
                    StatusId = item.StatusId,
                    Name = item.Name,
                    CreatedDate = item.CreatedDate,
                    ModifiedDate = item.ModifiedDate,
                    Description = item.Description,
                    UserSkillId= item.UserSkillId

                });
            }
            return userSkillTasks;
        }

        [HttpPost]
        [Route("updateUserSkillTask")]
        public async Task<IActionResult> UpdateUserSkillTask(UserSkillTaskModel userSkillTask, int userSkillTaskId)
        {
            if(ModelState.IsValid)
            {
                await _userSkillsData.UpdateUserSkillTasks(new UserSkillTaskDataModel
                {
                    Id = userSkillTaskId,
                    StatusId = userSkillTask.StatusId,
                    Name = userSkillTask.Name,
                    Description = userSkillTask.Description,
                    ModifiedDate = DateTime.UtcNow
                });
                return Ok("Updated successfully");
            }
            return BadRequest("Invalid request");
        }
    }
}
