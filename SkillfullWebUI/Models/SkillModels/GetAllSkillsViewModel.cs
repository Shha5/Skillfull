namespace SkillfullWebUI.Models.SkillModels
{
    public class GetAllSkillsViewModel
    {
        public string SearchPhrase {  get; set; }
        public List<SkillModel> Skills { get; set; }
    }
}
