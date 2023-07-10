namespace SkillfullWebUI.Constants
{
    public class SkillfullApiEndpoints
    {
        public const string BaseUrl = "https://localhost:7071/";

        //AUTH
        public const string ConfirmEmail = "api/Auth/ConfirmEmail";
        public const string ChangePassword = "api/Auth/ChangePassword";
        public const string CheckIfTokenIsValid = "api/Auth/CheckIfTokenIsValid";
        public const string ForgotPassword = "api/Auth/ForgotPassword";
        public const string Login = "api/Auth/Login";
        public const string RefreshToken = "api/Auth/RefreshToken";
        public const string Register = "api/Auth/Register";
        public const string ResendEmailConfirmation = "api/Auth/ResendEmailConfirmation";
        public const string ResetPassword = "api/Auth/ResetPassword";

        //SKILLS
        public const string GetAllSkills = "api/Skills/GetAllSkills";
        public const string GetSkillDetailsById = "api/Skills/GetSkillDetailsById";

        //USERSKILLS
        public const string AddUserSkill = "api/UserSkills/AddUserSkill";
        public const string GetAllUserSkills = "api/UserSkills/GetAllUserSkills";
        public const string UpdateUserSkill = "api/UserSkills/UpdateUserSkill";
        public const string DeleteUserSkill = "api/UserSkills/DeleteUserSkill";
        public const string AddTask = "api/UserSkills/AddTask";
        public const string GetAllTasksByUserId = "api/UserSkills/GetAllTasksByUserId";
        public const string GetAllTasksByUserSkillId = "api/UserSkills/GetAllTasksByUserSkillId";
        public const string ModifyTask = "api/UserSkills/ModifyTask";
        public const string DeleteTask = "api/UserSkills/DeleteTask";
    }
}
