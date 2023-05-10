using DataAccessLibrary.Models;

namespace DataAccessLibrary.Data.Interfaces
{
    public interface IUserSkillsData
    {
        Task AddUserSkill(string userId, UserSkillDataModel userSkill);
        List<UserSkillDataModel> GetUserSkills(string userId);
        //Task UpdateUserSkillAssessment(int userSkillAssessment);
    }
}