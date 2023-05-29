CREATE PROCEDURE [dbo].[sp_Delete_UserSkillTasks]
	@UserSkillTaskId INT
AS
BEGIN
DELETE
FROM [dbo].[UserSkillTasks]
WHERE @UserSkillTaskId = [UserSkillTasks].[Id]
END

