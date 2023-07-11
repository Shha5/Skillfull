using System.ComponentModel.DataAnnotations;

namespace SkillfullWebUI.Models.UserSkillsModels
{
    public class AddTaskModel
    {
        [Required]
        public string TaskName { get; set; }
   
        public string? TaskDescription { get; set; } = string.Empty;
        public string TaskStatusId { get; set; } = string.Empty;
        [Required]
        public string UserSkillName { get; set; }
        [Required]
        public string UserSkillId { get; set; }

    }
}
