using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SkillfullAPI.Models;
using SkillfullAPI.Models.DTOs;
using SkillfullAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SkillfullAPI.Controllers
{
    [Route("api/[controller]")] // api/Auth
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ISendGridEmailService _sendGridEmailService;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration, ISendGridEmailService sendGridEmailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _sendGridEmailService = sendGridEmailService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto registerRequestDto)
        {
            //Validate incoming request
            if(ModelState.IsValid)
            {
                //check if email exists
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
                else
                {
                    //create new user

                    var newUser = new IdentityUser()
                    {
                        Email = registerRequestDto.Email,
                        UserName = registerRequestDto.Name,
                        EmailConfirmed = false
                    };

                    var isCreated = await _userManager.CreateAsync(newUser, registerRequestDto.Password);

                    if(isCreated.Succeeded)
                    {
                        var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                        var callbackUrl = Url.Action("ConfirmEmail", "Auth", new { userId = newUser.Id, emailConfirmationToken = emailConfirmationToken }, protocol: HttpContext.Request.Scheme); //change to redirect to mvc app view 
                        string subject = "Email verification for Skillfull";
                        string message = "You can verify your email by clicking this" + "<a href=\"" + callbackUrl + "\"> link</a>";

                        await _sendGridEmailService.SendEmailAsync(newUser.Email, subject, message);
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
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string emailConfirmationToken)
        {
            if(string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(emailConfirmationToken))
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
            else
            {
                return BadRequest(new AuthResultModel() { Result = false, Errors = new List<string>() { "Failed to confirm email address." } });
            }
        }

        [HttpGet]
        [Route("ResendEmailConfirmation")]
        public async Task<IActionResult> ResendEmailConfirmation(string email, string password)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
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
            if(user == null)
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
            if(passwordCorrect == false)
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

            if(user.EmailConfirmed == true)
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
            var callbackUrl = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, emailConfirmationToken = emailConfirmationToken }, protocol: HttpContext.Request.Scheme); //change to redirect to mvc app view 
            string subject = "Email verification for Skillfull";
            string message = "You can verify your email by clicking this" + "<a href=\"" + callbackUrl + "\"> link</a>";

            await _sendGridEmailService.SendEmailAsync(user.Email, subject, message);
            return Ok();

        }

        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto loginRequest)
        {
            if (ModelState.IsValid)
            {
                //check if user exists
                var user = await _userManager.FindByEmailAsync(loginRequest.Email);
                if(user == null)
                {
                    return BadRequest(new AuthResultModel()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Invalid payload"
                        }
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
                if(isCorrect == false)
                {
                    BadRequest(new AuthResultModel()
                    {
                        Result = false,
                        Errors = new List<string>()
                        {
                            "Invalid credentials"
                        }
                    });
                }
                var jwtToken = GenerateJwtToken(user);
                return Ok(new AuthResultModel()
                {
                    Result = true,
                    Token = jwtToken
                });
            }
            return BadRequest(new AuthResultModel()
            {
                Result = false,
                Errors = new List<string>()
                {
                    "Invalid payload"
                }
            });
        }



     private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler= new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

            //token descriptor

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, value: user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }),

                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }

        [HttpPost]
        [Route("SendGridTest")]
        public async Task<IActionResult> SendEmailTest(string recipentEmail)
        {
            string message = "Test message";
            string subject = "Test message subject";

            await _sendGridEmailService.SendEmailAsync(recipentEmail, subject, message);

            return Ok();
        }




    }
}
