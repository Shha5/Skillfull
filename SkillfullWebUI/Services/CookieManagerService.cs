using SkillfullWebUI.Constants;
using SkillfullWebUI.Models.AuthModels;
using SkillfullWebUI.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace SkillfullWebUI.Services
{
    public class CookieManagerService : ICookieManagerService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public CookieManagerService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void CreateAuthCookies(AuthResultModel authResult, bool rememberMe)
        {

            if (rememberMe == true)
            {
                _contextAccessor.HttpContext.Response.Cookies.Append(CookieNames.Token, authResult.Token, new CookieOptions
                {
                    Domain = "localhost",
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(30),
                    IsEssential = true,
                    Secure = true
                });
                _contextAccessor.HttpContext.Response.Cookies.Append(CookieNames.RememberMe, "true", new CookieOptions
                {
                    Domain = "localhost",
                    HttpOnly = false,
                    Expires = DateTime.UtcNow.AddDays(30),
                    IsEssential = true,
                });

                _contextAccessor.HttpContext.Response.Cookies.Append(CookieNames.RefreshToken, authResult.RefreshToken, new CookieOptions
                {
                    Domain = "localhost",
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(30),
                    IsEssential = true,
                    Secure = true
                });

                _contextAccessor.HttpContext.Response.Cookies.Append(CookieNames.UserId, authResult.UserId, new CookieOptions
                {
                    Domain = "localhost",
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddDays(30),
                    IsEssential = true,
                    Secure = true
                });

                if (authResult.Username != null && _contextAccessor.HttpContext.Request.Cookies.ContainsKey(CookieNames.CookieConsent))
                {
                    _contextAccessor.HttpContext.Response.Cookies.Append(CookieNames.Username, authResult.Username, new CookieOptions
                    {
                        Domain = "localhost",
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddDays(30),
                        Secure = true
                    });
                }
            }
            else
            {
                _contextAccessor.HttpContext.Response.Cookies.Append(CookieNames.Token, authResult.Token, new CookieOptions
                {
                    Domain = "localhost",
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddHours(3),
                    IsEssential = true,
                    Secure = true
                });

                _contextAccessor.HttpContext.Response.Cookies.Append(CookieNames.RefreshToken, authResult.RefreshToken, new CookieOptions
                {
                    Domain = "localhost",
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddHours(3),
                    IsEssential = true,
                    Secure = true
                });
                _contextAccessor.HttpContext.Response.Cookies.Append(CookieNames.UserId, authResult.UserId, new CookieOptions
                {
                    Domain = "localhost",
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddHours(3),
                    IsEssential = true,
                    Secure = true
                });

                if (authResult.Username != null && _contextAccessor.HttpContext.Request.Cookies.ContainsKey(CookieNames.CookieConsent))
                {
                    _contextAccessor.HttpContext.Response.Cookies.Append(CookieNames.Username, authResult.Username, new CookieOptions
                    {
                        Domain = "localhost",
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddHours(3),
                        Secure = true
                    });
                }
            }
        }

        public void RemoveAuthCookies()
        {
            if (_contextAccessor.HttpContext.Request.Cookies.ContainsKey(CookieNames.Token))
            {
                _contextAccessor.HttpContext.Response.Cookies.Delete(CookieNames.Token);
            }
            if (_contextAccessor.HttpContext.Request.Cookies.ContainsKey(CookieNames.RefreshToken))
            {
                _contextAccessor.HttpContext.Response.Cookies.Delete(CookieNames.RefreshToken);
            }
            if (_contextAccessor.HttpContext.Request.Cookies.ContainsKey(CookieNames.UserId))
            {
                _contextAccessor.HttpContext.Response.Cookies.Delete(CookieNames.UserId);
            }
            if (_contextAccessor.HttpContext.Request.Cookies.ContainsKey(CookieNames.RememberMe))
            {
                _contextAccessor.HttpContext.Response.Cookies.Delete(CookieNames.RememberMe);
            }
            if (_contextAccessor.HttpContext.Request.Cookies.ContainsKey(CookieNames.Username))
            {
                _contextAccessor.HttpContext.Response.Cookies.Delete(CookieNames.Username);
            }
        }

        public bool AreAuthCookiesPresent()
        {
            var tokenCookiePresent = _contextAccessor.HttpContext.Request.Cookies.ContainsKey(CookieNames.Token);
            var refreshTokenCookiePresent = _contextAccessor.HttpContext.Request.Cookies.ContainsKey(CookieNames.RefreshToken);
            var userIdCookiePresent = _contextAccessor.HttpContext.Request.Cookies.ContainsKey(CookieNames.UserId);

            if(tokenCookiePresent == true && refreshTokenCookiePresent == true && userIdCookiePresent == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsRememberMeCookiePresent()
        {
            return _contextAccessor.HttpContext.Request.Cookies.ContainsKey(CookieNames.RememberMe);
        }

        public AuthCookiesValuesModel GetAuthCookieValues()
        {
            return new AuthCookiesValuesModel
            {
                Token = _contextAccessor.HttpContext.Request.Cookies[CookieNames.Token],
                RefreshToken = _contextAccessor.HttpContext.Request.Cookies[CookieNames.RefreshToken],
                UserId = _contextAccessor.HttpContext.Request.Cookies[CookieNames.UserId]
            };
        }
    }
}
