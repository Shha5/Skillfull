namespace SkillfullAPI.Models.LightcastApiModels
{
    public class SkillDetailsModel
    {
        public SkillCategoryModel Category { get; set; }
        public string Description { get; set; }
        public string DescriptionSource { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string InfoUrl { get; set; }
        public SkillCategoryModel Subcategory { get; set; }
        public SkillTypeModel Type { get; set; }
    }
}


    
