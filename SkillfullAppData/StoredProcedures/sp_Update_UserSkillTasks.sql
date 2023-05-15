CREATE PROCEDURE [dbo].[sp_Update_UserSkillTasks]
	@Name NVARCHAR(50),
	@Description NVARCHAR(50) = NULL,
	@Status INT = 1,
	@Id INT

AS
BEGIN

UPDATE UserSkillTasks
SET [Name] = @Name, [Description] = @Description, [StatusId] = @Status, [ModifiedDate] = getdate()
WHERE [Id] = @Id

END
