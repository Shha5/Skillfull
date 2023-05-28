CREATE PROCEDURE [dbo].[sp_Delete_UserSkills]
	@UserSkillId int
	
AS
BEGIN
	DELETE
	FROM [dbo].[UserSkills]
	WHERE @UserSkillId = [UserSkills].[Id]
END 
