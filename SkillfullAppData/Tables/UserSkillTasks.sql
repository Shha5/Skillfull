CREATE TABLE [dbo].[UserSkillTasks]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserSkillId] INT NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Description] NVARCHAR(50) NULL, 
    [Status] INT NOT NULL, 
    [CreatedDate] DATETIME2 NOT NULL DEFAULT getdate(), 
    [ModifiedDate] DATETIME2 NOT NULL DEFAULT getdate(), 
    CONSTRAINT [FK_UserSkillTasks_UserSkills] FOREIGN KEY ([UserSkillId]) REFERENCES [UserSkills]([Id]), 
    CONSTRAINT [FK_UserSkillTasks_TaskStatus] FOREIGN KEY ([Status]) REFERENCES [TaskStatus]([Id])
)
