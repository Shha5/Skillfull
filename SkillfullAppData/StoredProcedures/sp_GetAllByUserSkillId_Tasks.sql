CREATE PROCEDURE [dbo].[sp_GetAllByUserSkillId_Tasks]
	@userSkillId NVARCHAR(50)
AS
BEGIN
SET NOCOUNT ON
	SELECT [Tasks].[Id], [Tasks].[Name], [Tasks].[Description], [Tasks].[StatusId], [Tasks].[CreatedDate], [Tasks].[ModifiedDate],
	 [Tasks].[UserSkillId], [Tasks].[UserSkillName]
FROM Tasks
WHERE @userSkillId = [Tasks].[UserSkillId]

END