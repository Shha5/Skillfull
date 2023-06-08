CREATE PROCEDURE [dbo].[sp_GetAllForIdPerUser_UserSkillTasks]
	@userId NVARCHAR(50)
AS
BEGIN
SET NOCOUNT ON
	SELECT [UserSkillTasks].[Id], [UserSkillTasks].[Name], [UserSkillTasks].[Description], [UserSkillTasks].[StatusId], [UserSkillTasks].[CreatedDate], [UserSkillTasks].[ModifiedDate],
	 [UserSkillTasks].[UserSkillId], [UserSkillTasks].[UserSkillName]
FROM UserSkillTasks


END
