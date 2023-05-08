namespace SkillfullAPI.Models.LightcastApiModels
{
    public class SkillModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string infoUrl { get; set; }
        public SkillTypeModel type { get; set; }
    }

    public class SkillModelData
    {
        public SkillModel[] data { get; set; }
    }
}
