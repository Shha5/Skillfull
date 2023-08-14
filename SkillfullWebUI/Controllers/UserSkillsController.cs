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
        private readonly ICookieManagerService _cookieManager;

        public UserSkillsController(ILogger<UserSkillsController> logger, IApiService apiService, ICookieManagerService cookieManager)
        {
            _logger = logger;
            _apiService = apiService;
            _cookieManager = cookieManager;
        }


        public IActionResult AddUserSkill(string? skillId = null, string? skillName = null)
        {
            if (string.IsNullOrEmpty(skillId) || string.IsNullOrEmpty(skillName))
            {
                return View("Error");
            }
            ViewBag.SkillName = HttpUtility.UrlDecode(skillName);
            AddUserSkillViewModel addUserSkill = new AddUserSkillViewModel();
            return View(addUserSkill);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserSkill(AddUserSkillViewModel addUserSkill)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            if (_cookieManager.AreAuthCookiesPresent() == false)
            {
                ViewBag.ErrorMessage = "You must be logged in to perform this action.";
                return RedirectToAction("Login", "Auth");
            }  
            var result = await _apiService.AddUserSkill(addUserSkill);
            if (result.Result == true)
            {
                return View("AddUserSkillSuccess");
            }
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserSkills()
        {
            if (_cookieManager.AreAuthCookiesPresent() == false)
            {
                ViewBag.ErrorMessage = "You must be logged in to perform this action.";
                return View();
            }
            var response = await _apiService.GetAllUserSkills();
            if(response.Result == false)
            {
                return View("Error");
            }
            if (response.Result == true && response.Content == null)
            {
                ViewBag.ErrorMessage = "You haven't added any skills yet";
                return View();
            }
            return View(response.Content);
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
            if (_cookieManager.AreAuthCookiesPresent() == false)
            {
                ViewBag.ErrorMessage = "You must be logged in to perform this action.";
                return View();
            }

            var response = await _apiService.UpdateUserSkill(updateUserSkill.UserSkillId, updateUserSkill.NewSkillAssessmentId);
            if (response.Result == false)
            {
                return View("Error");
            }
            return View("UpdateUserSkillSuccess");
        }

        public async Task<IActionResult> DeleteUserSkill(string? userSkillId = null)
        {
            if (string.IsNullOrEmpty(userSkillId))
            {
                return View("Error");
            }
            if (_cookieManager.AreAuthCookiesPresent() == false)
            {
                ViewBag.ErrorMessage = "You must be logged in to perform this action.";
                return View();
            }
          
            var result = await _apiService.DeleteUserSkill(userSkillId);
            if (result.Result == false)
            {
                return View("Error");
            }
            return View("DeleteUserSkillSuccess");
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
        public async Task<IActionResult> AddUserSkillTask(AddTaskModel addUserSkillTask)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Not all necessary information have been provided";
                return View();
            }
            if (_cookieManager.AreAuthCookiesPresent() == false)
            {
                return RedirectToAction("Login", "Auth");
            }
            var result = await _apiService.AddTask(addUserSkillTask);
            if (result.Result == false)
            {
                return View("Error");
            }
            return View("AddUserSkillTaskSuccess");

        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasksByUserId()
        {
   
            var response = await _apiService.GetAllTasksByUserId();
            if (response.Result == false)
            {
                return View("Error");
            }
            if (response.Content == null)
            {
                ViewBag.ErrorMessage = "You haven't added any skills yet";
                return View();
            }
           
            return View("GetAllUserSkillTasks_User",response.Content);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasksByUserSkillId(string? userSkillId = null, string? userSkillName = null)
        {
            if (string.IsNullOrEmpty(userSkillId) || string.IsNullOrEmpty(userSkillName))
            {
                return View("Error");
            }
            if (_cookieManager.AreAuthCookiesPresent() == false)
            {
                return RedirectToAction("Login", "Auth");
            }
           
            var response = await _apiService.GetAllTasksByUserSkillId(userSkillId);
            if (response.Result == false)
            {
                return View("Error");
            }
            if (response.Content == null)
            {
                ViewBag.ErrorMessage = "You haven't added any skills yet";
                return View();
            }  
            return View("GetAllUserSkillTasks_Skill", response.Content);
        }

        public IActionResult UpdateUserSkillTask(string? userSkillTaskId = null, string? taskName = null)
        {
            if (string.IsNullOrEmpty(userSkillTaskId) || string.IsNullOrEmpty(taskName))
            {
                return View("Error");
            }
            ModifyTaskModel updateUserSkillTask = new ModifyTaskModel();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ModifyTask([FromForm]ModifyTaskModel updateTask)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            if (_cookieManager.AreAuthCookiesPresent() == false)
            {
                ViewBag.ErrorMessage = "You must be logged in to perform this action.";
                return View();
            }
            var response = await _apiService.ModifyTask(updateTask);
            if (response.Result == false)
            {
                return View("Error");
            }
            return View("UpdateUserSkillSuccess");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserSkillTask(string? userSkillTaskId = null)
        {
            if (string.IsNullOrEmpty (userSkillTaskId))
            {
                return View("Error");
            }
            if (_cookieManager.AreAuthCookiesPresent() == false)
            {
                ViewBag.ErrorMessage = "You must be logged in to perform this action.";
                return View();
            }
            var response = await _apiService.DeleteTask(userSkillTaskId);
            if (response.Result == false)
            {
                return View("Error");
            }
            return View("DeleteUserSkillTaskSuccess");
        }
    }
}
