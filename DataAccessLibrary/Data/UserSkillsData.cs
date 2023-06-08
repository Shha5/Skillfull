using DataAccessLibrary.Data.Interfaces;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;


namespace DataAccessLibrary.Data
{
    public class UserSkillsData : IUserSkillsData
    {
        private readonly ISqlDataAccess _sqlDataAccess;

        public UserSkillsData(ISqlDataAccess sqlDataAccess)
        {
            _sqlDataAccess = sqlDataAccess;
        }

        public List<UserSkillDataModel> GetUserSkills(string userId)
        {
            return _sqlDataAccess.LoadData<UserSkillDataModel, dynamic>("dbo.sp_GetAllForId_UserSkills", new { userId }).Result.ToList();
        }


        public Task AddUserSkill(string userId, UserSkillDataModel userSkill) =>
             _sqlDataAccess.SaveData("dbo.sp_Add_UserSkills", new { userId, userSkill.SkillName, userSkill.SkillId, userSkill.SkillAssessmentId });

        public List<UserSkillTaskDataModel> GetUserSkillTasks(string userSkillId)
        {
            return _sqlDataAccess.LoadData<UserSkillTaskDataModel, dynamic>("dbo.sp_GetAllForId_UserSkillTasks", new { userSkillId }).Result.ToList();
        }

        public List<UserSkillTaskDataModel> GetUserSkillTasksPerUser(string userId)
        {
            return _sqlDataAccess.LoadData<UserSkillTaskDataModel, dynamic>("dbo.sp_GetAllForIdPerUser_UserSkillTasks", new { userId }).Result.ToList();
        }

        public Task AddUserSkillTask(string userId, UserSkillTaskDataModel userSkillTask) =>
             _sqlDataAccess.SaveData("dbo.sp_Add_UserSkillTasks", new { userSkillTask.UserSkillId, userSkillTask.UserSkillName, userSkillTask.Name, userSkillTask.Description, userSkillTask.StatusId, userId });


        public Task UpdateUserSkillAssessment(int userSkillId, int skillAssessmentId) =>
            _sqlDataAccess.SaveData("dbo.sp_Update_UserSkills", new { userSkillId, skillAssessmentId });

        public Task UpdateUserSkillTasks(UserSkillTaskDataModel userSkillTask) =>
            _sqlDataAccess.SaveData("dbo.sp_Update_UserSkillTasks", new { userSkillTask.Id, userSkillTask.Name, userSkillTask.Description, userSkillTask.StatusId });

        public Task DeleteUserSkills(int userSkillId) =>
            _sqlDataAccess.SaveData("dbo.sp_Delete_UserSkills", new { userSkillId });

        public Task DeleteUserSkillTasks(int userSkillTaskId) =>
            _sqlDataAccess.SaveData("dbo.sp_Delete_UserSkillTasks", new { userSkillTaskId });

    }
}


