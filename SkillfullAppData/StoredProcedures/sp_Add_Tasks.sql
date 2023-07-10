CREATE PROCEDURE [dbo].[sp_Add_Tasks]
	@Name NVARCHAR(50),
	@Description NVARCHAR(50) = NULL,
	@StatusId INT = 1,
	@UserSkillId NVARCHAR(50),
	@UserId NVARCHAR(50),
	@UserSkillName NVARCHAR(150)

AS
BEGIN
	INSERT INTO [Tasks] ([Name], [Description], [StatusId], [UserSkillId], [UserId], [UserSkillName])
	VALUES (@Name, @Description, @StatusId, @UserSkillId, @UserId, @UserSkillName)
END


