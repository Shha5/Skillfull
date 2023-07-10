CREATE PROCEDURE [dbo].[sp_Delete_Tasks]
	@UserSkillTaskId INT
AS
BEGIN
DELETE
FROM [dbo].[Tasks]
WHERE @UserSkillTaskId = [Tasks].[Id]
END

