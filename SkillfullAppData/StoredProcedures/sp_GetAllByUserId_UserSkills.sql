CREATE PROCEDURE [dbo].[sp_GetAllByUserId_UserSkills]
	@userId NVARCHAR(50)
AS
BEGIN
SET NOCOUNT ON

	SELECT [UserSkills].[Id], [UserSkills].[SkillId], [UserSkills].[SkillAssessmentId],[UserSkills].[TargetSkillAssessmentId], [UserSkills].[SkillName]

	FROM [UserSkills]


	WHERE [UserSkills].UserId = @userId

END