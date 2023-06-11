using Microsoft.AspNetCore.Mvc;
using SkillfullWebUI.Models.UserSkillsModels;
using SkillfullWebUI.Services.Interfaces;
using System.Web;

namespace SkillfullWebUI.Controllers
{
    public class UserSkillsController : Controller
    {
        private readonly ILogger<UserSkillsController> _logger;
        private readonly IApiService _apiService;

        public UserSkillsController(ILogger<UserSkillsController> logger, IApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }


        public IActionResult AddUserSkill(string? skillId = null, string? skillName = null)
        {
            if (string.IsNullOrEmpty(skillId) || string.IsNullOrEmpty(skillName))
            {
                return View("Error");
            }
            ViewBag.SkillName = HttpUtility.UrlDecode(skillName);
            AddUserSkillModel addUserSkill = new AddUserSkillModel();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUserSkill(AddUserSkillModel addUserSkill)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            else
            {
                if (HttpContext.Request.Cookies.ContainsKey("UserId"))
                {
                    var result = await _apiService.AddUserSkill(HttpContext.Request.Cookies["UserId"], addUserSkill.SkillId, addUserSkill.SkillName, addUserSkill.SkillAssessmentId);
                    if (result.IsSuccessStatusCode)
                    {
                        return View("AddUserSkillSuccess");
                    }
                    else
                    {
                        return View("Error");
                    }
                }
                else
                {
                    return View("Error");
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserSkills()
        {
            if (HttpContext.Request.Cookies.ContainsKey("UserId"))
            {
                var result = await _apiService.GetAllUserSkills(HttpContext.Request.Cookies["UserId"]);
                if (result.Count > 0)
                {
                    return View(result);
                }
                else if(result.Count == 0)
                {
                    ViewBag.InfoMessage = "No skills added to profile";
                    return View();
                }
                else { return View("Error"); }
            } else { return View("Error"); }
        }

        public IActionResult UpdateUserSkill(string? userSkillId = null, string? skillName = null)
        {
            if (string.IsNullOrEmpty(userSkillId) || string.IsNullOrEmpty(skillName))
            {
                return View("Error");
            }
            ViewBag.SkillName = HttpUtility.UrlDecode(skillName);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserSkill(UpdateUserSkillModel updateUserSkill)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            var result = await _apiService.UpdateUserSkill(updateUserSkill.UserSkillId, updateUserSkill.NewSkillAssessmentId);
            if (result.IsSuccessStatusCode)
            {
                return View("UpdateUserskillSuccess");
            }
            return View("Error");
        }


        public async Task<IActionResult> DeleteUserSkill(string? userSkillId = null)
        {
            if (string.IsNullOrEmpty(userSkillId))
            {
                return View("Error");
            }
            var result = await _apiService.DeleteUserSkill(userSkillId);
            if (result.IsSuccessStatusCode)
            {

                return View("DeleteUserSkillSuccess");
            }
            return View("Error");
        }

        public IActionResult AddUserSkillTask(string? userSkillId = null, string? userSkillName = null)
        {
            if (string.IsNullOrEmpty(userSkillId) || string.IsNullOrEmpty(userSkillName))
            {
                return View("Error");
            }
            ViewBag.SkillName = HttpUtility.UrlDecode(userSkillName);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUserSkillTask(AddUserSkillTaskModel addUserSkillTask)
        {
           
            if(ModelState.IsValid)
            {
                if (HttpContext.Request.Cookies.ContainsKey("UserId"))
                {
                    var result = await _apiService.AddUserSkillTask(addUserSkillTask, HttpContext.Request.Cookies["UserId"]);
                    if (result.IsSuccessStatusCode)
                    {
                        return View("AddUserSkillTaskSuccess");
                    }
                    return View("Error");
                }
            }
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserSkillTasks_User()
        {
            if (HttpContext.Request.Cookies.ContainsKey("UserId"))
            {
                var response = await _apiService.GetAllUserSkillTasks_User(HttpContext.Request.Cookies["UserId"]);
                if(response.Count > 0 && response != null)
                {
                    return View(response);
                }
                return View("Error");
            }
            return View("Error");   
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserSkillTasks_Skill(string? userSkillId = null, string? userSkillName = null)
        {
            if (HttpContext.Request.Cookies.ContainsKey("UserId"))
            {
                var response = await _apiService.GetAllUserSkillTasks_Skill(userSkillId);
                if (response.Count > 0 && response != null)
                {
                    ViewBag.InfoMessage = string.Concat("Tasks for", userSkillName);
                    return View(response);
                }
                return View("Error");
            }
            return View("Error");
        }
    }
}
