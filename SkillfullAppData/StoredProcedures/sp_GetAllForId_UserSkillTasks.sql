CREATE PROCEDURE [dbo].[sp_GetAllForId_UserSkillTasks]
	@userSkillId int
AS
BEGIN
SET NOCOUNT ON
	SELECT [UserSkillTasks].[Id], [UserSkillTasks].[Name], [UserSkillTasks].[Description], [UserSkillTasks].[Status], [UserSkillTasks].[CreatedDate], [UserSkillTasks].[ModifiedDate],
	[TaskStatus].[Name]
FROM UserSkillTasks
JOIN TaskStatus ON [UserSkillTasks].[Status] = [TaskStatus].[Id]

WHERE [UserSkillTasks].[UserSkillId] = @userSkillId
END
