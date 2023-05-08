namespace SkillfullAPI.Models.AppModels
{
    public class UserSkillModel
    {
        public int? Id { get; set; } = null;
        public string SkillId { get; set; }
        public string SkillName { get; set; }
        public int SkillAssessment { get; set; }
    }
}
