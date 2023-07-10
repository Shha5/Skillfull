namespace SkillfullWebUI.Models.UserSkillsModels
{
    public class UpdateUserSkillTaskModel
    {
        public string UserSkillTaskId { get; set; }
        public string NewTaskName { get; set; }

        public string? NewTaskDescription { get; set; } = string.Empty;
        public string NewTaskStatusId { get; set; } = string.Empty;

    }
}
