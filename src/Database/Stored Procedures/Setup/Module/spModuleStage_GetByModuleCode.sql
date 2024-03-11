CREATE PROCEDURE [dbo].[spModuleStage_GetByModuleCode]
@moduleCode NVARCHAR(150)
AS
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
	WHERE m.Code = @moduleCode
	ORDER BY ms.[Level]
RETURN 0
