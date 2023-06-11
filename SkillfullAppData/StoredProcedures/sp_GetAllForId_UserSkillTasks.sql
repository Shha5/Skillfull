CREATE PROCEDURE [dbo].[sp_GetAllForId_UserSkillTasks]
	@userSkillId NVARCHAR(50)
AS
BEGIN
SET NOCOUNT ON
	SELECT [UserSkillTasks].[Id], [UserSkillTasks].[Name], [UserSkillTasks].[Description], [UserSkillTasks].[StatusId], [UserSkillTasks].[CreatedDate], [UserSkillTasks].[ModifiedDate],
	 [UserSkillTasks].[UserSkillId], [UserSkillTasks].[UserSkillName]
FROM UserSkillTasks
WHERE @userSkillId = [UserSkillTasks].[UserSkillId]

END