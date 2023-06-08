using Microsoft.AspNetCore.Mvc;
using SkillfullAPI.Models.AppModels;
using DataAccessLibrary.Data.Interfaces;
using DataAccessLibrary.Models;
using System.Web;

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
        public async Task<IActionResult> AddUserSkill([FromForm] UserSkillModel userSkillModel, [FromForm] string userId)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(userId))
            {
                UserSkillDataModel userSkill = new UserSkillDataModel()
                {
                    SkillId = userSkillModel.SkillId,
                    SkillName = HttpUtility.UrlDecode(userSkillModel.SkillName),
                    Id = null,
                    SkillAssessmentId = userSkillModel.SkillAssessmentId
                };

               await _userSkillsData.AddUserSkill(userId, userSkill);
                return Ok();
            } 
            else
            {
                return BadRequest("Request not valild.");
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
                    UserSkillId = item.Id,
                    SkillId = item.SkillId,
                    SkillName = item.SkillName,
                    SkillAssessmentId = item.SkillAssessmentId
                });
            }
            return userSkills;
        }

        [HttpPost]
        [Route("updateUserSkill")]
        public async Task<IActionResult> UpdateUserSkill([FromForm] int userSkillId,[FromForm] int newSkillAssessmentId)
        {
            if (newSkillAssessmentId == null || newSkillAssessmentId <= 0 || newSkillAssessmentId > 5 || userSkillId == null)
            {
                return BadRequest("Request not valid");
            }
            await _userSkillsData.UpdateUserSkillAssessment(userSkillId, newSkillAssessmentId);
            return Ok("Successfully updated.");
        }

        [HttpPost]
        [Route("deleteUserSkill")]
        public async Task<IActionResult> DeleteUserSkill([FromForm]int userSkillId)
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
        public async Task<IActionResult> AddUserSkillTask([FromForm] string userId, [FromForm] UserSkillTaskModel userSkillTaskModel)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(userId))
            {
                UserSkillTaskDataModel userSkillTask = new UserSkillTaskDataModel()
                {
                    Name = userSkillTaskModel.TaskName,
                    Description = userSkillTaskModel.TaskDescription,
                    StatusId = userSkillTaskModel.TaskStatusId,
                    UserSkillId = userSkillTaskModel.UserSkillId,
                    UserSkillName = userSkillTaskModel.UserSkillName,
                    UserId = userId
                };
                try
                {
                    await _userSkillsData.AddUserSkillTask(userId, userSkillTask);
                }
                catch(Exception ex)
                {
                    return NotFound(ex.Message);
                }
                return Ok("Task successfully added to user and skill.");
            }
            else
            {
                return BadRequest("Request not valid");
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
                    TaskStatusId = item.StatusId,
                    TaskName = item.Name,
                    TaskCreatedDate = item.CreatedDate,
                    TaskModifiedDate = item.ModifiedDate,
                    TaskDescription = item.Description,
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
                    StatusId = userSkillTask.TaskStatusId,
                    Name = userSkillTask.TaskName,
                    Description = userSkillTask.TaskDescription,
                    ModifiedDate = DateTime.UtcNow
                });
                return Ok("Updated successfully");
            }
            return BadRequest("Invalid request");
        }

        [HttpPost]
        [Route("deleteUserSkillTask")]
        public async Task<IActionResult> DeleteUserSkillTask(int userSkillTaskId)
        {
            if(userSkillTaskId == null)
            {
                return BadRequest();
            }
            await _userSkillsData.DeleteUserSkillTasks(userSkillTaskId);
            return Ok();
        }
    }
}
