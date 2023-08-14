using System.ComponentModel.DataAnnotations;

namespace SkillfullWebUI.Models.UserSkillsModels
{
    public class TaskModel
    {
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string? TaskDescription { get; set; } = string.Empty;
        public string TaskStatusId { get; set; } = string.Empty;
        public string UserSkillName { get; set; }
        public string UserSkillId { get; set; }
    }
}
