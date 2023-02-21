CREATE PROCEDURE [dbo].[sp_GetAllForId_UserSkills]
	@userId NVARCHAR(50)
AS
BEGIN
SET NOCOUNT ON

	SELECT [UserSkills].[Id], [UserSkills].[SkillId], [UserSkills].[SkillAssessmentId], [UserSkills].[SkillName], 
	[UserSkillTasks].[Name], [UserSkillTasks].[Description], [UserSkillTasks].[CreatedDate], [UserSkillTasks].[ModifiedDate], [UserSkillTasks].[Status],
	[TaskStatus].[Name]

	FROM [UserSkills]

	JOIN [UserSkillTasks]
	ON [UserSkills].[Id] = [UserSkillTasks].[UserSkillId]

	JOIN [TaskStatus]
	ON [UserSkillTasks].[Status] = [TaskStatus].[Id]

END