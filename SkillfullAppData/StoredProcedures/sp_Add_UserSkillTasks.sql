CREATE PROCEDURE [dbo].[sp_Add_UserSkillTasks]
	@Name NVARCHAR(50),
	@Description NVARCHAR(50) = NULL,
	@StatusId INT = 1,
	@userSkillId NVARCHAR(50),
	@userId NVARCHAR(50)

AS
BEGIN
	INSERT INTO UserSkillTasks ([Name], [Description], [StatusId], [UserSkillId], [UserId])
	VALUES (@Name, @Description, @StatusId, @userSkillId, @userId)
END


