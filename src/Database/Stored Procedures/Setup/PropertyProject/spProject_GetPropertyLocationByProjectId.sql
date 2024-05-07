CREATE PROCEDURE [dbo].[spProject_GetPropertyLocationByProjectId]


	@id int  
AS
	SELECT 
		pp.*,
	ppl.LocationId  
FROM PropertyProject pp
 LEFT JOIN PropertyProjectLocation ppl ON ppl.ProjectId = pp.Id
 Where pp.Id = @id
RETURN 0
