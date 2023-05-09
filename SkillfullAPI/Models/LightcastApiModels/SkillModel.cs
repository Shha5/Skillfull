namespace SkillfullAPI.Models.LightcastApiModels
{
    public class SkillModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string InfoUrl { get; set; }
        public SkillTypeModel Type { get; set; }
    }
}
