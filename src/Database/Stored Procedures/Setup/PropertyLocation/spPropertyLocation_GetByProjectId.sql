CREATE PROCEDURE [dbo].[spPropertyLocation_GetByProjectId]
	@projectId int,
    @developerId int
AS
 



IF @projectId IS NULL 
BEGIN 

    SELECT 
        pl.*
    FROM PropertyLocation pl
    LEFT JOIN (
        SELECT Distinct
		    ppl.LocationId
        FROM PropertyProjectLocation ppl
        INNER JOIN PropertyProject pp 
            ON pp.Id = ppl.ProjectId 
            AND pp.CompanyId = @developerId
    ) pc 
    ON pc.LocationId = pl.Id
END
ELSE
Begin 
 SELECT 
   pp.CompanyId,
    pl.*,
    ppl.ProjectId
FROM PropertyLocation pl
LEFT JOIN PropertyProjectLocation ppl ON ppl.LocationId = pl.Id
LEFT JOIN PropertyProject pp ON pp.Id = ppl.ProjectId
WHERE 
1 = (CASE WHEN @projectId IS NULL then 1
          WHEN @projectId IS NOT NULL AND ppl.ProjectId = @projectId  THEN 1
	END ) 
AND
	1 = (CASE WHEN @developerId IS NULL then 1
          WHEN @developerId IS NOT NULL AND pp.CompanyId = @developerId  THEN 1
		END )  
End
 

 

RETURN 0
