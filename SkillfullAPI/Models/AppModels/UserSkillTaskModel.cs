using DataAccessLibrary.Models;

namespace SkillfullAPI.Models.AppModels
{
    public class UserSkillTaskModel
    {
        public int? TaskId { get; set; } = null;
        public string TaskName { get; set; }
        public string? TaskDescription { get; set; } = string.Empty;
        public int TaskStatusId { get; set; } = 1;
        public DateTime? TaskCreatedDate { get; set; }
        public DateTime? TaskModifiedDate { get; set; }
        public int UserSkillId { get; set; }
        public string UserSkillName { get; set; }
    }
}
