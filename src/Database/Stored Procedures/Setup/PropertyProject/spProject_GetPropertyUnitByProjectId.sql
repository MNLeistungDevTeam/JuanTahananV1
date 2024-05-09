CREATE PROCEDURE [dbo].[spProject_GetPropertyUnitByProjectId]
	@id int  
AS
	SELECT 
		pp.*,
		ppl.UnitId  
FROM PropertyProject pp
 LEFT JOIN PropertyUnitProject ppl ON ppl.ProjectId = pp.Id
 Where pp.Id = @id
RETURN 0