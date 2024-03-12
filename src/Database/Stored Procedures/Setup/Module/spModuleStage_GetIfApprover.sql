CREATE PROCEDURE [dbo].[spModuleStage_GetIfApprover]
	@userId int,
	@moduleCode nvarchar(50)
AS
	SELECT 
		ms.*
	FROM ModuleStage ms
	INNER JOIN Module m ON m.Id = ms.ModuleId
	WHERE ms.ApproverId = @userId and m.Code = @moduleCode
RETURN 0
