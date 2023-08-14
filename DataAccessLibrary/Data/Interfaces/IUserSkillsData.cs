using DataAccessLibrary.Models;

namespace DataAccessLibrary.Data.Interfaces
{
    public interface IUserSkillsData
    {
        Task AddUserSkill(string userId, UserSkillDataModel userSkill);
        Task AddTask(string userId, TaskDataModel task);
        Task DeleteUserSkill(int userSkillId);
        Task DeleteTask(int taskId);
        List<UserSkillDataModel> GetUserSkills(string userId); 
        List<TaskDataModel> GetTasksByUserId(string userId);
        List<TaskDataModel> GetTasksForUserSkillId(string userSkillId);
        Task ModifyTask(TaskDataModel task);
        Task UpdateUserSkillAssessment(int userSkillId, int skillAssessmentId);
    }
}
