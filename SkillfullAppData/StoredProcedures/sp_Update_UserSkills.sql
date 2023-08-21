CREATE PROCEDURE [dbo].[sp_Update_UserSkills]
	@userSkillId INT,
	@skillAssessmentId INT,
	@targetSkillAssessmentId INT = NULL

AS
BEGIN
	UPDATE UserSkills
	SET [SkillAssessmentId] = @skillAssessmentId,
		[TargetSkillAssessmentId] = @targetSkillAssessmentId
	WHERE @userSkillId = UserSkills.[Id]
END
