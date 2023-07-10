CREATE PROCEDURE [dbo].[sp_GetAllByUserId_Tasks]
	@userId NVARCHAR(50)
AS
BEGIN
SET NOCOUNT ON
	SELECT [Tasks].[Id], [Tasks].[Name], [Tasks].[Description], [Tasks].[StatusId], [Tasks].[CreatedDate], [Tasks].[ModifiedDate],
	 [Tasks].[UserSkillId], [Tasks].[UserSkillName]
FROM Tasks
WHERE @userId = [Tasks].[UserId]

END
