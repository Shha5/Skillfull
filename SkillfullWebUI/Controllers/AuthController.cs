using Microsoft.AspNetCore.Mvc;
using SkillfullWebUI.Models.AuthModels;
using SkillfullWebUI.Services.Interfaces;

namespace SkillfullWebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiService _apiService;

        public AuthController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegistrationRequestModel registerViewModel = new RegistrationRequestModel();
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestModel registrationRequest)
        {
            if (ModelState.IsValid)
            {
               var result = await _apiService.Register(registrationRequest);
                if(result.IsSuccessStatusCode)
                {
                    return View("RegistrationSuccess");
                }
                else
                {
                    ViewBag.ErrorMessage = "Something went wrong";
                    return View("Register");
                }
            }
            
            return View("Register");
        }

        [HttpGet]
        public IActionResult ConfirmEmail(string emailConfirmationToken = null, string userId = null)
        {
            if (emailConfirmationToken == null || userId == null)
            {
                return View("Error");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationModel emailConfirmation)
        {
            if(ModelState.IsValid)
            {
                var result = await _apiService.ConfirmEmail(emailConfirmation);
                if (result.IsSuccessStatusCode)
                {
                    return View("EmailConfirmationSuccess");
                }
                else
                {
                    ViewBag.ErrorMessafe = "Verification failed";
                    return View("ResendEmailConfirmation");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Verification link is not valid.";
                return View("ResendEmailConfirmation");
            }
        }

        [HttpGet]
        public IActionResult ResendEmailConfirmation()
        {
            ResendEmailConfirmationModel resendEmailConfirmation = new ResendEmailConfirmationModel();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationModel resendEmailConfirmation)
        {
            if (ModelState.IsValid)
            {
                var result = await _apiService.ResendEmailConfirmation(resendEmailConfirmation);
                
                if(result.IsSuccessStatusCode) 
                {
                    return View("ResendEmailConfirmationSuccess");
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to resend email confirmation";
                    return View();
                }
            }
            ViewBag.ErrorMessage = "Invalid credentials";
            return View();
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            LoginModel login = new LoginModel();
            login.ReturnUrl = returnUrl ?? Url.Content("~/");
            return View(login);
        }

        public async Task<IActionResult> Login(LoginModel login, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                login.ReturnUrl = returnUrl;
                returnUrl = returnUrl ?? Url.Content("~/");
            
                var result = await _apiService.Login(login);
                if(result.IsSuccessStatusCode)
                {
                    result
                    return LocalRedirect(returnUrl);
                }
            }
            return View();
        }
    }

}
