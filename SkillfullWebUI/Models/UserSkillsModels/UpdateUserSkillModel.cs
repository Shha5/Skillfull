using System.ComponentModel.DataAnnotations;

namespace SkillfullWebUI.Models.UserSkillsModels
{
    public class UpdateUserSkillModel
    {
        [Required]
        public string UserSkillId { get; set; }

        [Required]
        public string NewSkillAssessmentId { get; set; }

        [Required]
        public string NewTargetSkillAssessmentId { get; set; }
    }
}
