CREATE PROCEDURE [dbo].[sp_Add_UserSkills]
	@userId NVARCHAR(50),
	@skillId NVARCHAR(50),
	@skillName NVARCHAR(150),
	@skillAssessmentId INT = NULL

AS
BEGIN
	INSERT INTO UserSkills ([UserId], [SkillId], [SkillName], [SkillAssessmentId])
	VALUES (@userId, @skillId, @skillName, @skillAssessmentId)
END
