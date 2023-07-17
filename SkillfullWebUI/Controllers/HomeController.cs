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


        //TODO: work on pagination of search results
        [HttpGet]
        public async Task<IActionResult> GetAllSkills(string? searchPhrase, int pg = 1)
        {
            const int pageSize = 20;
            List<SkillModel> skills = new();
            var result = await _apiService.GetAllSkills();
            if(string.IsNullOrEmpty(searchPhrase))
            {
                skills = result.Content;
                if (pg < 1)
                    pg = 1;
                int recsCount = skills.Count();
                var pager = new Pager(recsCount, pg, pageSize);
                int recSkip = (pg - 1) * pageSize;
                var data = skills.Skip(recSkip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;

                return View(data);
            }
            else
            {
                var searchResult = result.Content.Where(skill => skill.Name.ToLower().Contains(searchPhrase.ToLower())); 
                skills = searchResult.ToList();
                return View(skills);
            }
            

            

        }

        [HttpGet]
        public async Task<IActionResult> GetSkillDetails(string? skillId)
        {
            var result = await _apiService.GetSkillDetailsById(skillId);
            return View(result.Content);
        }
        
    }
}