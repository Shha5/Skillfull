using System.ComponentModel.DataAnnotations;

namespace SkillfullWebUI.Models.AuthModels
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress] 
        public string Email { get; set; }
    }
}
