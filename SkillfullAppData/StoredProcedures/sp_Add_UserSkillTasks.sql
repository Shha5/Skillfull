CREATE PROCEDURE [dbo].[sp_Add_UserSkillTasks]
	@Name NVARCHAR(50),
	@Description NVARCHAR(50) = NULL,
	@Status INT = 1,
	@userSkillId NVARCHAR(50)

AS
BEGIN
	INSERT INTO UserSkillTasks ([Name], [Description], [Status], [UserSkillId])
	VALUES (@Name, @Description, @Status, @userSkillId)
END


