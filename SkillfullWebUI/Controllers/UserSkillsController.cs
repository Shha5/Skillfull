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
            if(string.IsNullOrEmpty(skillId) || string.IsNullOrEmpty(skillName))
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
            if(!ModelState.IsValid)
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

        public async Task<IActionResult> GetAllUserSkills()
        {
            if (HttpContext.Request.Cookies.ContainsKey("UserId"))
            {
                var result = await _apiService.GetAllUserSkills(HttpContext.Request.Cookies["UserId"]);
                if(result.Count > 0)
                {
                    return View(result);
                }
                else { return View("Error"); }
            } else { return View("Error"); }
        }

    }
}
