CREATE PROCEDURE [dbo].[spPropertyUnit_GetUnitByProjectId]
	@projectId INT,
	@developerId INT
AS
SELECT * FROM PropertyUnit pu
 LEFT JOIN
 (SELECT DISTINCT pup.UnitId,pp.CompanyId,pp.Id ProjectId  FROM
   PropertyUnitProject pup

   LEFT JOIN PropertyProject pp ON pp.Id = pup.ProjectId 
   )pr ON pr.UnitId = pu.Id
   
   WHERE pr.CompanyId = @developerId AND
    1 = CASE WHEN @projectId IS NULL THEN 1 
              WHEN @projectId IS NOT NULL AND @projectId = pr.ProjectId  THEN 1
		  END
RETURN 0
