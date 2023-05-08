namespace SkillfullAPI.Models.LightcastApiModels
{
    public class SkillDetailsModel
    {
        public SkillCategoryModel category { get; set; }
        public string description { get; set; }
        public string descriptionSource { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string infoUrl { get; set; }
        public SkillCategoryModel subcategory { get; set; }
        public SkillTypeModel type { get; set; }
    }

    public class SkillDetailsModelData
    {
        public SkillDetailsModel data { get; set; }
    }
}
