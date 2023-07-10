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

        public Task AddUserSkill(string userId, UserSkillDataModel userSkill) =>
             _sqlDataAccess.SaveData("dbo.sp_Add_UserSkills", new { userId, userSkill.SkillName, userSkill.SkillId, userSkill.SkillAssessmentId });
        public Task AddTask(string userId, TaskDataModel task) =>
             _sqlDataAccess.SaveData("dbo.sp_Add_Tasks", new { task.UserSkillId, task.UserSkillName, task.Name, task.Description, task.StatusId, userId });

        public Task DeleteUserSkill(int userSkillId) =>
            _sqlDataAccess.SaveData("dbo.sp_Delete_UserSkills", new { userSkillId });

        public Task DeleteTask(int taskId) =>
            _sqlDataAccess.SaveData("dbo.sp_Delete_Tasks", new { taskId });

        public List<UserSkillDataModel> GetUserSkills(string userId)
        {
            return _sqlDataAccess.LoadData<UserSkillDataModel, dynamic>("dbo.sp_GetAllByUserId_UserSkills", new { userId }).Result.ToList();
        }

        public List<TaskDataModel> GetTasksByUserId(string userId)
        {
            return _sqlDataAccess.LoadData<TaskDataModel, dynamic>("dbo.sp_GetAllByUserId_Tasks", new { userId }).Result.ToList();
        }

        public List<TaskDataModel> GetTasksForUserSkillId(string userSkillId)
        {
            return _sqlDataAccess.LoadData<TaskDataModel, dynamic>("dbo.sp_GetAllByUserSkillId_Tasks", new { userSkillId }).Result.ToList();
        }

        public Task ModifyTask(TaskDataModel task) =>
    _sqlDataAccess.SaveData("dbo.sp_Modify_Tasks", new { task.Id, task.Name, task.Description, task.StatusId });


        public Task UpdateUserSkillAssessment(int userSkillId, int skillAssessmentId) =>
            _sqlDataAccess.SaveData("dbo.sp_Update_UserSkills", new { userSkillId, skillAssessmentId });



        

    }
}


