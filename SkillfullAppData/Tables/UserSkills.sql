CREATE TABLE [dbo].[UserSkills]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SkillId] NVARCHAR(50) NOT NULL, 
    [SkillAssessmentId] INT NULL, 
    [UserId] NVARCHAR(50) NOT NULL, 
    [SkillName] NVARCHAR(150) NOT NULL, 
    CONSTRAINT [FK_UserSkills_SkillAssessments] FOREIGN KEY ([SkillAssessmentId]) REFERENCES [SkillAssessments]([Id])
)
