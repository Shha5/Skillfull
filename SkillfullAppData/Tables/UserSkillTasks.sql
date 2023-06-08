CREATE TABLE [dbo].[UserSkillTasks]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [UserId] NVARCHAR(50) NOT NULL,
    [UserSkillId] INT NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Description] NVARCHAR(300) NULL, 
    [StatusId] INT NOT NULL, 
    [CreatedDate] DATETIME2 NOT NULL DEFAULT getdate(), 
    [ModifiedDate] DATETIME2 NOT NULL DEFAULT getdate(), 
    [UserSkillName] NVARCHAR(150) NOT NULL,
    CONSTRAINT [FK_UserSkillTasks_UserSkillsId] FOREIGN KEY ([UserSkillId]) REFERENCES [UserSkills]([Id]), 
    CONSTRAINT [FK_UserSkillTasks_TaskStatusId] FOREIGN KEY ([StatusId]) REFERENCES [TaskStatus]([Id]) 
)
