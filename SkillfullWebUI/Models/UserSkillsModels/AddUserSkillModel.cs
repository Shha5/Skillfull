using System.ComponentModel.DataAnnotations;

namespace SkillfullWebUI.Models.UserSkillsModels
{
    public class AddUserSkillModel
    {
        [Required]
        public string SkillId { get; set; }

        [Required]
        public string SkillName { get; set; }

        [Required]
        public string SkillAssessmentId { get; set; }
    }
}
