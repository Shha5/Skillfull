using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SkillfullAPI.Data;
using SkillfullAPI.Models.AuthModels;
using SkillfullAPI.Models.AuthModels.DTOs;
using SkillfullAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SkillfullAPI.Services
{
    public class JwtTokenGenerationService : IJwtTokenGenerationService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly TokenValidationParameters _validationParameters;
        private readonly UserManager<IdentityUser> _userManager;

        public JwtTokenGenerationService(IConfiguration configuration, ApplicationDbContext context, TokenValidationParameters validationParameters, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _context = context;
            _validationParameters = validationParameters;
            _userManager = userManager;
        }

        public async Task<AuthResultModel> GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);
            var expiryTimeFrame = TimeSpan.Parse(_configuration.GetSection("JwtConfig:ExpiryTimeFrame").Value);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, value: user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
                }),

                Expires = DateTime.UtcNow.Add(expiryTimeFrame),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = new RefreshTokenModel()
            {
                JwtId = token.Id,
                Token = GenerateRandomString(23),
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                IsRevoked = false,
                IsUsed = false,
                UserId = user.Id,
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResultModel()
            {
                Result = true,
                Token = jwtToken,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthResultModel> VerifyAndGenerateToken(TokenRequestDto tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                _validationParameters.ValidateLifetime = false; //for testing

                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _validationParameters, out var validatedToken);
                var unixExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expiryDateTime = UnixTimeStampToDateTime(unixExpiryDate);
                var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if(result == false)
                    {
                        return null;
                    }
                }

                if(expiryDateTime < DateTime.UtcNow || storedToken.ExpiryDate < DateTime.UtcNow)
                {
                    return new AuthResultModel()
                    {
                        Result = false,
                        Errors = new List<string>
                        {
                            "Token expired"
                        }
                    };
                }

                if (storedToken == null || storedToken.IsUsed || storedToken.IsRevoked || storedToken.JwtId != jti)
                {
                    return new AuthResultModel()
                    {
                        Result = false,
                        Errors = new List<string>
                        {
                            "Invalid tokens"
                        }
                    };
                }

                storedToken.IsUsed = true;

                 _context.RefreshTokens.Update(storedToken);
                await _context.SaveChangesAsync();

                var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);

                return await GenerateJwtToken(dbUser);
            }
            catch (Exception ex)
            {
                return new AuthResultModel()
                {
                    Result = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        private string GenerateRandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPRSTUWVQXYZabcdefghijklmnoprstuwvqxyz1234567890_";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTimeValue = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeValue = dateTimeValue.AddSeconds(unixTimeStamp);
            return dateTimeValue;
        }
    }
}
