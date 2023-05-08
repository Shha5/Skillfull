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
            return _sqlDataAccess.LoadData<UserSkillDataModel, dynamic>("dbo.sp_GetAllForId_UserSkills", new { }).Result.ToList();
        }


        public Task AddUserSkill(string userId, UserSkillDataModel userSkill) =>
             _sqlDataAccess.SaveData("dbo.sp_Add_UserSkills", new { userId, userSkill.SkillName, userSkill.SkillId, userSkill.SkillAssessment });


        public Task UpdateUserSkillAssessment(int userSkillAssessment) =>
            _sqlDataAccess.SaveData("dbo.sp_Update_UserSkills", new { userSkillAssessment });

    }
}


