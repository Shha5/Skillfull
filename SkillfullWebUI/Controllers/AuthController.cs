using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
        public IActionResult ChangePassword()
        {
            if (HttpContext.Request.Cookies.ContainsKey("userId"))
            {
                ChangePasswordModel changePassword = new ChangePasswordModel();
                return View();
            }
            else
            {
                return View("Error");
            }    
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePassword)
        {
            if (ModelState.IsValid)
            {
                string userId = HttpContext.Request.Cookies["UserId"];
                var result = await _apiService.ChangePassword(changePassword, userId);
                if (result.IsSuccessStatusCode)
                {
                    return View("ChangePasswordSuccess");
                }
                else
                {
                    ViewBag.ErrorMessage = "Something went wrong";
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Please provide all the necessary data";
                return View();
            }
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

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                login.ReturnUrl = returnUrl;
                returnUrl = returnUrl ?? Url.Content("~/");
            
                var result = await _apiService.Login(login);
                if(result == null)
                {

                    return View();
                }
                if(result.Result == true)
                {
                    Response.Cookies.Append("token", result.Token, new CookieOptions
                    {
                        Domain = "localhost",
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddMinutes(20),
                        IsEssential = true,
                        Secure = true
                    });
                    

                    if (login.RememberMe == true)
                    {
                        Response.Cookies.Append("RememberMe", "true", new CookieOptions
                        {
                            Domain = "localhost",
                            HttpOnly = false,
                            Expires = DateTime.UtcNow.AddDays(30),
                            IsEssential = true,
                        });

                        Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
                        {
                            Domain = "localhost",
                            HttpOnly = true,
                            Expires = DateTime.UtcNow.AddDays(30),
                            IsEssential = true,
                            Secure = true
                        });
                        Response.Cookies.Append("UserId", result.UserId, new CookieOptions
                        {
                            Domain = "localhost",
                            HttpOnly = true,
                            Expires = DateTime.UtcNow.AddDays(30),
                            IsEssential = true,
                            Secure = true
                        });
                    }
                    else
                    {
                        Response.Cookies.Append("refreshToken", result.RefreshToken, new CookieOptions
                        {
                            Domain = "localhost",
                            HttpOnly = true,
                            Expires = DateTime.UtcNow.AddHours(3),
                            IsEssential = true,
                            Secure = true
                        });
                        Response.Cookies.Append("UserId", result.UserId, new CookieOptions
                        {
                            Domain = "localhost",
                            HttpOnly = true,
                            Expires = DateTime.UtcNow.AddHours(3),
                            IsEssential = true,
                            Secure = true
                        });
                    }
                  
                    if (result.Username != null && HttpContext.Request.Cookies.ContainsKey(".AspNet.Consent"))
                    {
                        Response.Cookies.Append("Username", result.Username, new CookieOptions
                        {
                            Domain = "localhost",
                            HttpOnly = true,
                            Expires = DateTime.UtcNow.AddDays(20),
                            Secure = true
                        });
                    }
                    return LocalRedirect(returnUrl);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            ForgotPasswordModel forgotPassword = new ForgotPasswordModel();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPassword)
        {
            if (ModelState.IsValid)
            {
                var result = await _apiService.ForgotPassword(forgotPassword.Email);
                if (result.IsSuccessStatusCode)
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetPassword)
        {
            if (ModelState.IsValid)
            {
                var result = await _apiService.ResetPassword(resetPassword);
                if (result.IsSuccessStatusCode)
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
            if(HttpContext.Request.Cookies.ContainsKey("refreshToken"))
            {
                HttpContext.Response.Cookies.Delete("refreshToken");
                if (HttpContext.Request.Cookies.ContainsKey("token"))
                {
                    HttpContext.Response.Cookies.Delete("token");
                }
                if (HttpContext.Request.Cookies.ContainsKey("Username"))
                {
                    HttpContext.Response.Cookies.Delete("Username");
                }
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("Index", "Home");
        }
    }

    
}
