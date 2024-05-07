CREATE PROCEDURE [dbo].[spPropertyLocation_GetByProjectId]
	@projectId int 
AS
	SELECT  
		pl.*,
		ppl.ProjectId
	FROM PropertyLocation pl
	LEFT JOIN PropertyProjectLocation ppl ON ppl.LocationId = pl.Id
	Where ppl.ProjectId = @projectId

RETURN 0
