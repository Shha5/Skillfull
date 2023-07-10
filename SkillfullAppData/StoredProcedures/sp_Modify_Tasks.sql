CREATE PROCEDURE [dbo].[sp_Modify_Tasks]
	@Name NVARCHAR(50),
	@Description NVARCHAR(50) = NULL,
	@StatusId INT = 1,
	@Id INT

AS
BEGIN

UPDATE Tasks
SET [Name] = @Name, [Description] = @Description, [StatusId] = @StatusId, [ModifiedDate] = getdate()
WHERE [Id] = @Id

END
