namespace DataAccessLibrary.Models
{
    public class UserSkillTaskDataModel
    {
        public int? Id { get; set; } = null;
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; } = null;
        public DateTime? ModifiedDate { get; set; } = null;
        public int StatusId { get; set; } = 1;
        public int UserSkillId { get; set; }
        public string UserId { get; set; }
    }
}
