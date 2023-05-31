using System.ComponentModel.DataAnnotations;

namespace SkillfullWebUI.Models.AuthModels
{
    public class EmailConfirmationModel
    {
        [Required]
        public string EmailConfirmationToken { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
