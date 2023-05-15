//using DataAccessLibrary.DataAccess;
//using DataAccessLibrary.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DataAccessLibrary.Data
//{
//    public class UserSkillTasksData
//    {

//        private readonly ISqlDataAccess _sqlDataAccess;

//        public UserSkillTasksData(ISqlDataAccess sqlDataAccess)
//        {
//            _sqlDataAccess = sqlDataAccess;
//        }

//        public List<UserSkillTaskDataModel> GetUserSkillTasks(string userSkillId)
//        {
//            return _sqlDataAccess.LoadData<UserSkillTaskDataModel, dynamic>("dbo.sp_GetAllForId_UserSkillTasks", new { }).Result.ToList();
//        }


//        public Task AddUserSkillTask(string userSkillId, UserSkillTaskDataModel userSkillTask) =>
//             _sqlDataAccess.SaveData("dbo.sp_Add_UserSkillTasks", new { userSkillId, userSkillTask.Name, userSkillTask.Description, userSkillTask.Status, userId });


//        //public Task UpdateUserSkillAssessment(int userSkillAssessment) =>
//        //    _sqlDataAccess.SaveData("dbo.sp_Update_UserSkills", new { userSkillAssessment });
//    }
//}
