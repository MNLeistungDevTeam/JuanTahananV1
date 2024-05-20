CREATE PROCEDURE [dbo].[spProject_GetByCompanyId]
	@companyId INT,
    @locationId INT
 
AS
	
IF @locationId IS NULL
BEGIN
    SELECT  
        pp.*,
        c.[Name] AS CompanyName
    FROM PropertyProject pp 
    LEFT JOIN Company c ON c.Id = pp.CompanyId
    WHERE pp.CompanyId = @companyId;
END
ELSE
BEGIN
    SELECT  
        pp.*,
        c.[Name] AS CompanyName,
        ppl.LocationId AS LocationId
    FROM PropertyProject pp 
    LEFT JOIN Company c ON c.Id = pp.CompanyId
    LEFT JOIN PropertyProjectLocation ppl ON pp.Id = ppl.ProjectId
    WHERE pp.CompanyId = @companyId
    AND (
        @locationId IS NULL OR
        (@locationId IS NOT NULL AND ppl.LocationId = @locationId)
    );
END;

RETURN 0
