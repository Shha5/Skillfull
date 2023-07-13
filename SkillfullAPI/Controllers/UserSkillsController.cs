using Microsoft.AspNetCore.Mvc;
using SkillfullAPI.Models.AppModels;
using DataAccessLibrary.Data.Interfaces;
using DataAccessLibrary.Models;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SkillfullAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [Route("AddUserSkill")]
      
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
        [Route("GetAllUserSkills")]
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
        [Route("UpdateUserSkill")]
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
        [Route("DeleteUserSkill")]
        public async Task<IActionResult> DeleteUserSkill([FromForm]int userSkillId)
        {
            if(userSkillId == null)
            {
                return BadRequest("Invalid request");
            }
            await _userSkillsData.DeleteUserSkill(userSkillId);
            return Ok("Record deleted permanently.");
        }

        [HttpPost]
        [Route("AddTask")]
        public async Task<IActionResult> AddUserSkillTask([FromForm] string userId, [FromForm] TaskModel taskModel)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(userId))
            {
                TaskDataModel userSkillTask = new TaskDataModel()
                {
                    Name = taskModel.TaskName,
                    Description = taskModel.TaskDescription,
                    StatusId = taskModel.TaskStatusId,
                    UserSkillId = taskModel.UserSkillId,
                    UserSkillName = taskModel.UserSkillName,
                    UserId = userId
                };
                try
                {
                    await _userSkillsData.AddTask(userId, userSkillTask);
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
        [Route("GetAllTasksByUserId")]
        public async Task<List<TaskModel>> GetAllTasksForUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }
            var result = _userSkillsData.GetTasksByUserId(userId).ToList();
            List<TaskModel> userSkillTasks = new List<TaskModel>();
            if (result.Count == 0)
            {
                return null;
            }
            foreach (var item in result)
            {
                userSkillTasks.Add(new TaskModel()
                {
                    TaskId = item.Id,
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

        [HttpGet]
        [Route("GetAllTasksByUserSkillId")]
        public async Task<List<TaskModel>> GetAllTasksByUserSkillId(string userSkillId)
        {
            if (string.IsNullOrEmpty(userSkillId))
            {
                return null;
            }
            var result = _userSkillsData.GetTasksForUserSkillId(userSkillId).ToList();
            List<TaskModel> userSkillTasks = new List<TaskModel>();
            if (result.Count == 0)
            {
                return null;
            }
            foreach (var item in result)
            {
                userSkillTasks.Add(new TaskModel()
                {
                    TaskId = item.Id,
                    TaskStatusId = item.StatusId,
                    TaskName = item.Name,
                    TaskCreatedDate = item.CreatedDate,
                    TaskModifiedDate = item.ModifiedDate,
                    TaskDescription = item.Description,
                    UserSkillId = item.UserSkillId

                }) ;
            }
            return userSkillTasks;
        }

        [HttpPost]
        [Route("ModifyTask")]
        public async Task<IActionResult> ModifyTask([FromForm]ModifyTaskModel userSkillTask)
        {
            if(ModelState.IsValid)
            {
                await _userSkillsData.ModifyTask(new TaskDataModel
                {
                    Id = userSkillTask.UserSkillTaskId,
                    StatusId = userSkillTask.NewTaskStatusId,
                    Name = userSkillTask.NewTaskName,
                    Description = userSkillTask.NewTaskDescription,
                    ModifiedDate = DateTime.UtcNow
                });
                return Ok("Updated successfully");
            }
            return BadRequest("Invalid request");
        }

        [HttpPost]
        [Route("DeleteTask")]
        public async Task<IActionResult> DeleteTask(int userSkillTaskId)
        {
            if(userSkillTaskId == null)
            {
                return BadRequest();
            }
            await _userSkillsData.DeleteTask(userSkillTaskId);
            return Ok();
        }
    }
}
