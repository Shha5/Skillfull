namespace DataAccessLibrary.Models
{
    public class UserSkillDataModel
    {
        public int? Id { get; set; } = null;
        public string SkillId { get; set; }
        public string SkillName { get; set; }
        public int? SkillAssessmentId { get; set; }
        public int? TargetSkillAssessmentId { get; set; }

    }
}
