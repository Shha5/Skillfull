using SkillfullWebUI.Models.AuthModels;

namespace SkillfullWebUI.Services.Interfaces
{
    public interface ICookieManagerService
    {
        void CreateAuthCookies(AuthResultModel authResult, bool rememberMe);
        void RemoveAuthCookies();
        bool AreAuthCookiesPresent();
        AuthCookiesValuesModel GetAuthCookieValues();
        bool IsRememberMeCookiePresent();
    }
}