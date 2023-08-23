using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SkillfullAPI.Models.AuthModels;
using SkillfullAPI.Models.AuthModels.DTOs;
using SkillfullAPI.Services.Interfaces;
using System.Net;
using System.Web;

namespace SkillfullAPI.Controllers
{
    [Route("api/[controller]")] // api/Auth
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ISendGridEmailService _sendGridEmailService;
        private readonly ITokenGenerationService _jwtTokenGenerationService;
        
        public AuthController(UserManager<IdentityUser> userManager, ISendGridEmailService sendGridEmailService, ITokenGenerationService jwtTokenGenerationService)
        {
            _userManager = userManager;
            _sendGridEmailService = sendGridEmailService;
            _jwtTokenGenerationService = jwtTokenGenerationService;
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string emailConfirmationToken)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(emailConfirmationToken))
            {
                return BadRequest(new AuthResultModel()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Failed to confirm email"
                    }
                });
            }
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, emailConfirmationToken);
            if (result.Succeeded)
            {
                return Ok();
            }
            else if (result.Errors.Any())
            {
                List<string> errors = new List<string>();
                foreach (var error in result.Errors)
                {
                    errors.Add(error.ToString());
                }
                return BadRequest(new AuthResultModel() { Result = false, Errors = errors });
            }
            else
            {
                return BadRequest(new AuthResultModel() { Result = false, Errors = new List<string>() { "Unspecified error occured." } });
            }
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromForm] PasswordChangeRequestDto passwordChangeRequest)
        {
            if (ModelState.IsValid) 
            {
                var user = await _userManager.FindByIdAsync(passwordChangeRequest.UserId);
                var result = await _userManager.ChangePasswordAsync(user, passwordChangeRequest.CurrentPassword, passwordChangeRequest.NewPassword);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new AuthResultModel()
                    {
                        Result = false,
                        Errors = new List<string>() { result.Errors.ToString() }
                    });
                }  
            }
            else
            {
                return BadRequest(new AuthResultModel()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Data provided is not complete"
                    }
                });
            }
        }

        [HttpPost]
        [Route("CheckIfTokenIsValid")]
        public  async Task<IActionResult> CheckIfTokenIsValid([FromForm]string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("No token was provided");
            }
            var result = await _jwtTokenGenerationService.IsTokenValid(token);
            return Ok(result);
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromForm]string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new AuthResultModel()
                {
                    Result = false,
                    Errors = new List<string>() { "Invalid request" }
                });
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new AuthResultModel() { Result = false, Errors = new List<string>() { "Invalid request" } });
            }
            else
            {
                var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedPasswordResetToken = HttpUtility.UrlEncode(passwordResetToken);
                var callbackUrl = string.Concat("https://localhost:7154/Auth/ResetPassword?", "userId=", user.Id, "&passwordResetToken=", encodedPasswordResetToken); //change to redirect to mvc app view 
                string subject = "Password reset for your Skillfull account";
                string message = "You can reset your password by clicking this" + "<a href=\"" + callbackUrl + "\"> link</a>";

                await _sendGridEmailService.SendEmailAsync(user.Email, subject, message);
                return Ok();
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromForm] UserLoginRequestDto loginRequest)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginRequest.Email);
                if (user == null)
                {
                    return BadRequest(new AuthResultModel()
                    {
                        Result = false,
                        Errors = new List<string>() { "Invalid credentials" }
                    });
                }
                var isCorrect = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
                if (isCorrect == false)
                {
                    return BadRequest(new AuthResultModel()
                    {
                        Result = false,
                        Errors = new List<string>() { "Invalid credentials" }
                    });
                }
                var jwtToken = await _jwtTokenGenerationService.GenerateJwtToken(user);
                return Ok(new AuthResultModel()
                {
                    Token = jwtToken.Token,
                    RefreshToken = jwtToken.RefreshToken,
                    Result = true,
                    Username = user.UserName,
                    UserId = user.Id
                });
            }
            return BadRequest(new AuthResultModel()
            {
                Result = false,
                Errors = new List<string>() { "Invalid credentials" }
            });
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromForm] TokenRequestDto tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _jwtTokenGenerationService.VerifyAndGenerateToken(tokenRequest);

                if (result == null)
                {
                    return BadRequest(new AuthResultModel()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Verification failed"
                        }
                    });
                }
                return Ok(result);
            }
            return BadRequest(new AuthResultModel()
            {
                Result = false,
                Errors = new List<string>()
                {
                    "Invalid parameters"
                }
            });
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm] UserRegistrationRequestDto registerRequestDto)
        {
            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByEmailAsync(registerRequestDto.Email);
                if (userExists != null)
                {
                    return BadRequest(new AuthResultModel()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Email exists"
                        }
                    });
                }
                var newUser = new IdentityUser()
                {
                    Email = registerRequestDto.Email,
                    UserName = registerRequestDto.Name,
                    EmailConfirmed = false
                };
                var isCreated = await _userManager.CreateAsync(newUser, registerRequestDto.Password);
                if (isCreated.Succeeded)
                {
                    var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var encodedEmailConfirmationToken = WebUtility.UrlEncode(emailConfirmationToken);
                    var callbackUrl = string.Concat("https://localhost:7154/Auth/ConfirmEmail?", "userId=", newUser.Id, "&emailConfirmationToken=", encodedEmailConfirmationToken); //FIX THIS - when deploying the host will change
                    string subject = "Email verification for Skillfull";
                    string message = "You can verify your email by clicking this" + "<a href=\"" + callbackUrl + "\"> link</a>";
                    await _sendGridEmailService.SendEmailAsync(newUser.Email, subject, message);
                    return Ok(new AuthResultModel()
                    {
                        Result = true
                    });
                }
                else
                {
                    return BadRequest(new AuthResultModel()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Server error"
                        }
                    });
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("ResendEmailConfirmation")]
        public async Task<IActionResult> ResendEmailConfirmation([FromForm] string email, [FromForm] string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest(new AuthResultModel()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Invalid credentials"
                    }
                });
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new AuthResultModel()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Invalid credentials"
                    }
                });
            }
            var passwordCorrect = await _userManager.CheckPasswordAsync(user, password);
            if (passwordCorrect == false)
            {
                return BadRequest(new AuthResultModel()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Invalid credentials"
                    }
                });
            }
            if (user.EmailConfirmed == true)
            {
                return BadRequest(new AuthResultModel()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Invalid request"
                    }
                });
            }
            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedEmailConfirmationToken = HttpUtility.UrlEncode(emailConfirmationToken);
            var callbackUrl = string.Concat("https://localhost:7154/Auth/ConfirmEmail?", "userId=", user.Id, "&emailConfirmationToken=", encodedEmailConfirmationToken); //change to redirect to mvc app view 
            string subject = "Email verification for Skillfull";
            string message = "You can verify your email by clicking this" + "<a href=\"" + callbackUrl + "\"> link</a>";

            await _sendGridEmailService.SendEmailAsync(user.Email, subject, message);
            return Ok();
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] string userId, [FromForm] string passwordResetToken, [FromForm] string newPassword)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(passwordResetToken) || string.IsNullOrEmpty(newPassword))
            {
                return BadRequest(new AuthResultModel()
                {
                    Result = false,
                    Errors = new List<string>() { "Invalid request" }
                });
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest(new AuthResultModel()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Invalid request"
                    }
                });
            }
            var isReset = await _userManager.ResetPasswordAsync(user, passwordResetToken, newPassword);
            if (!isReset.Succeeded)
            {
                return BadRequest(new AuthResultModel()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Failed to reset password"
                    }
                });
            }
            return Ok();
        }
    }
}

