CREATE PROCEDURE [dbo].[spModuleStage_GetByModuleId]
	@id INT
AS
BEGIN
	SELECT
		ms.*,
		m.[Description] ModuleName,
		m.[Code] ModuleCode,
		ua.FirstName,
		ua.LastName,
		ua.MiddleName,
		ua.UserName
	FROM ModuleStage ms
	INNER JOIN Module m ON m.Id = ms.ModuleId
	INNER JOIN [User] ua ON ms.ApproverId = ua.Id
	WHERE m.Id = @id
	ORDER BY ms.[Level]
END
