CREATE PROCEDURE [dbo].[sp_Add_UserSkills]
	@userId NVARCHAR(50),
	@skillId NVARCHAR(50),
	@skillName NVARCHAR(150),
	@skillAssessmentId INT = NULL,
	@targetSkillAssessmentId INT = NULL

AS
BEGIN
	INSERT INTO UserSkills ([UserId], [SkillId], [SkillName], [SkillAssessmentId], [TargetSkillAssessmentId])
	VALUES (@userId, @skillId, @skillName, @skillAssessmentId, @targetSkillAssessmentId)
END
