using Microsoft.AspNetCore.Mvc;
using SkillfullWebUI.Models;
using SkillfullWebUI.Models.SkillModels;
using SkillfullWebUI.Services.Interfaces;
using System.Diagnostics;

namespace SkillfullWebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IApiService _apiService;

        public HomeController(ILogger<HomeController> logger, IApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSkills(string? searchPhrase)
        {
            var result = await _apiService.GetAllSkills();
            if(string.IsNullOrEmpty(searchPhrase))
            {
                return View(result);
            }
            else
            {
                var searchResult = result.Where(skill => skill.Name.ToLower().Contains(searchPhrase.ToLower())); 
                return View(searchResult.ToList());
            } 
        }

        [HttpGet]
        public async Task<IActionResult> GetSkillDetails(string? skillId)
        {
            var result = await _apiService.GetSkillDetailsById(skillId);
            return View(result);
        }
        
    }
}