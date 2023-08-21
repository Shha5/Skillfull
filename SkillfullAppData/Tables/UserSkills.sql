CREATE TABLE [dbo].[UserSkills]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SkillId] NVARCHAR(50) NOT NULL, 
    [SkillAssessmentId] INT NOT NULL DEFAULT 1, 
    [UserId] NVARCHAR(50) NOT NULL, 
    [SkillName] NVARCHAR(150) NOT NULL, 
    [TargetSkillAssessmentId] INT NOT NULL DEFAULT 3, 
    CONSTRAINT [FK_UserSkills_SkillAssessments] FOREIGN KEY ([SkillAssessmentId]) REFERENCES [SkillAssessments]([Id]), 
    CONSTRAINT [FK_UserSkills_TargetSkillAssessments] FOREIGN KEY ([TargetSkillAssessmentId]) REFERENCES [TargetSkillAssessments]([Id])
)
