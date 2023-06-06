//namespace SkillfullWebUI.Helpers
//{
//    public class CookieHelper
//    {
//        private readonly IHttpContextAccessor _contextAccessor;

//        public CookieHelper(IHttpContextAccessor contextAccessor)
//        {
//            _contextAccessor = contextAccessor;
//        }

//        Check if cookie is valid(is there[done], is not expired)
//        public bool IsCookieValid(string key)
//        {
//            return _contextAccessor.HttpContext.Request.Cookies.ContainsKey(key);
//        }
//    }
//}
