using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public IActionResult Attributions()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetAllSkills(string? searchPhrase, int pg = 1)
        {
            const int pageSize = 20;
            List<SkillModel> skills = new();

            var result = await _apiService.GetAllSkills();
            if(string.IsNullOrEmpty(searchPhrase))
            {
                skills = result.Content; 
            }
            else
            {
                var response = await _apiService.GetAllSkills();
                var searchResult = response.Content.Where(skill => skill.Name.ToLower().Contains(searchPhrase.ToLower()));
                skills = searchResult.ToList();  
            }

            if (pg < 1)
                pg = 1;
            int recsCount = skills.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = skills.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

            return View(new GetAllSkillsViewModel() { SearchPhrase = searchPhrase, Skills = data });
        }

       

        [HttpGet]
        public async Task<IActionResult> GetSkillDetails(string? skillId)
        {
            var result = await _apiService.GetSkillDetailsById(skillId);
            return View(result.Content);
        }
        
    }
}