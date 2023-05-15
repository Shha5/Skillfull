using DataAccessLibrary.Models;

namespace SkillfullAPI.Models.AppModels
{
    public class UserSkillTaskModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; } = 1;
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int UserSkillId { get; set; }
        public string UserId { get; set; }
    }
}
