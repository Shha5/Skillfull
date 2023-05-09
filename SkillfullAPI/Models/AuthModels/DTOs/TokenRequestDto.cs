using System.ComponentModel.DataAnnotations;

namespace SkillfullAPI.Models.AuthModels.DTOs
{
    public class TokenRequestDto
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
