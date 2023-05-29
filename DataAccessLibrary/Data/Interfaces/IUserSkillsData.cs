using DataAccessLibrary.Models;

namespace DataAccessLibrary.Data.Interfaces
{
    public interface IUserSkillsData
    {
        Task AddUserSkill(string userId, UserSkillDataModel userSkill);
        List<UserSkillDataModel> GetUserSkills(string userId);
        Task AddUserSkillTask(string userSkillId, UserSkillTaskDataModel userSkillTask);
        List<UserSkillTaskDataModel> GetUserSkillTasks(string userSkillId);
        List<UserSkillTaskDataModel> GetUserSkillTasksPerUser(string userId);
        Task UpdateUserSkillAssessment(int userSkillId, int skillAssessmentId);
        Task UpdateUserSkillTasks(UserSkillTaskDataModel userSkillTask);
        Task DeleteUserSkills(int userSkillId);
        Task DeleteUserSkillTasks(int userSkillTaskId);
    }
}