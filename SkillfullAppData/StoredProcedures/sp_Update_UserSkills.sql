CREATE PROCEDURE [dbo].[sp_Update_UserSkills]
	@userSkillId INT,
	@skillAssessmentId INT
AS
BEGIN
	UPDATE UserSkills
	SET [SkillAssessmentId] = @skillAssessmentId
	WHERE @userSkillId = UserSkills.[Id]
END
