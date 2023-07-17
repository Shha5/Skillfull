using Microsoft.AspNetCore.Mvc;
using SkillfullWebUI.Models.AuthModels;
using SkillfullWebUI.Services.Interfaces;

namespace SkillfullWebUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiService _apiService;
        private readonly ICookieManagerService _cookieManager;
       

        public AuthController(IApiService apiService, ICookieManagerService cookieManager)
        {
            _apiService = apiService;
            _cookieManager = cookieManager;
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
                if (result.Result == true)
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
        public IActionResult ChangePassword()
        {
            if (_cookieManager.AreAuthCookiesPresent() == true)
            {
                ChangePasswordModel changePassword = new ChangePasswordModel();
                return View(changePassword);
            }
            else
            {
                return RedirectToAction("Login");
            }
               
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePassword)
        {
            if (ModelState.IsValid)
            {
                if (_cookieManager.AreAuthCookiesPresent())
                {
                  
                    var result = await _apiService.ChangePassword(changePassword);
                    if (result.Result == true)
                    {
                        return View("ChangePasswordSuccess");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = result.ErrorMessage;
                        return View();
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "You must be logged in to change the password. If you forgot your password, please click the \"Forgot password?\" link below the login form.";
                    RedirectToAction("Login");
                }  
            }
            ViewBag.ErrorMessage = "Please provide all the necessary data";
            return View();
           
        }

        [HttpGet]
        public IActionResult ConfirmEmail(string emailConfirmationToken = null, string userId = null)
        {
            if (emailConfirmationToken == null || userId == null)
            {
                return View("Error");
            }
            EmailConfirmationModel emailConfirmation = new EmailConfirmationModel();
            return View(emailConfirmation);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationModel emailConfirmation)
        {
            if(ModelState.IsValid)
            {
                var result = await _apiService.ConfirmEmail(emailConfirmation);
                if (result.Result == true)
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
            return View(resendEmailConfirmation);
        }

        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationModel resendEmailConfirmation)
        {
            if (ModelState.IsValid)
            {
                var result = await _apiService.ResendEmailConfirmation(resendEmailConfirmation);
                
                if(result.Result == true) 
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

        public IActionResult Login()
        {
            LoginModel login = new LoginModel();
            return View(login);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = "~/";
                }
                else
                {
                    returnUrl = string.Concat("~/", returnUrl.Remove(0, 23));
                }
                var result = await _apiService.Login(login);
          
                if(result.Result == true)
                {
                    return LocalRedirect(returnUrl);
                }
                ViewBag.ErrorMessage = result.ErrorMessage;
                return View();
            }
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            ForgotPasswordModel forgotPassword = new ForgotPasswordModel();
            return View(forgotPassword);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPassword)
        {
            if (ModelState.IsValid)
            {
                var result = await _apiService.ForgotPassword(forgotPassword.Email);
                if (result.Result == true)
                {
                    return View("ForgotPasswordSuccess");
                }
                else
                {
                    ViewBag.ErrorMessage = "Couldn't send a reset password email.";
                    return View();
                }
            }
            ViewBag.ErrorMessage = "Please enter a valid email address.";
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string? userId = null, string? passwordResetToken = null )
        {
            if(userId == null || passwordResetToken == null)
            {
                return View("Error");
            }
            ResetPasswordModel resetPassword = new ResetPasswordModel();
            return View(resetPassword);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPassword)
        {
            if (ModelState.IsValid)
            {
                var result = await _apiService.ResetPassword(resetPassword);
                if (result.Result == true)
                {
                    return View("ResetPasswordSuccess");
                }
                else
                {
                    ViewBag.ErrorMessage = "Something went wrong.";
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Please verify that your new password meet the criteria";
                return View();
            }
        }

        public IActionResult Logout()
        {
            _cookieManager.RemoveAuthCookies();
            return RedirectToAction("Index", "Home");
        }
    }

    
}
