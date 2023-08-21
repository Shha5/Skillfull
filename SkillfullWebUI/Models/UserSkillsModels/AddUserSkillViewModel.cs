using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace SkillfullWebUI.Models.UserSkillsModels
{
    public class AddUserSkillViewModel
    {
        [Required]
        public string SkillId { get; set; }

        [Required]
        public string SkillName { get; set; }

        public int? SkillAssessmentId { get; set; }

        public int? TargetSkillAssessmentId { get; set; }
    }
}
