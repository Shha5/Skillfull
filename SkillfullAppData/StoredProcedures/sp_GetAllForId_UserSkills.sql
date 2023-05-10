CREATE PROCEDURE [dbo].[sp_GetAllForId_UserSkills]
	@userId NVARCHAR(50)
AS
BEGIN
SET NOCOUNT ON

	SELECT [UserSkills].[Id], [UserSkills].[SkillId], [UserSkills].[SkillAssessmentId], [UserSkills].[SkillName]

	FROM [UserSkills]


	WHERE [UserSkills].UserId = @userId

END