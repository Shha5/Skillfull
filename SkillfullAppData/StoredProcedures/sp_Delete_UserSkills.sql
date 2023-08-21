CREATE PROCEDURE [dbo].[sp_Delete_UserSkills]
	@UserSkillId INT
	
AS
BEGIN
	DELETE
	FROM [dbo].[UserSkills]
	WHERE @UserSkillId = [UserSkills].[Id]
END 
