using System.ComponentModel.DataAnnotations;

namespace SkillfullAPI.Models.AuthModels.DTOs
{
    public class PasswordChangeRequestDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

    }
}
