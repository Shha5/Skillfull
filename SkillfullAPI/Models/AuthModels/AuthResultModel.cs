using Microsoft.AspNetCore.Identity;

namespace SkillfullAPI.Models.AuthModels
{
    public class AuthResultModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Result { get; set; }
        public List<string> Errors { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
